using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.Events.Commands;
using MakerSchedule.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

using MediatR;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using MakerSchedule.Domain.Utilties.ImageUtilities;
using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.DTO.EventTag;
using MakerSchedule.Application.Constants;

public class PatchEventCommandHandler(IApplicationDbContext context, ILogger<PatchEventCommandHandler> logger, IImageStorageService imageStorageService) : IRequestHandler<PatchEventCommand, EventDTO>
{
     public async Task<EventDTO> Handle(PatchEventCommand request, CancellationToken cancellationToken)
    {
        var eventId = request.EventId;
        var eventDTO = request.EventDTO;
        var e = await context.Events
           .Include(ev => ev.Occurrences)
               .ThenInclude(o => o.Attendees)
                   .ThenInclude(a => a.User)
           .Include(ev => ev.Occurrences)
               .ThenInclude(o => o.Leaders)
                   .ThenInclude(l => l.User)
           .FirstOrDefaultAsync(ev => ev.Id == eventId);
        if (e == null)
        {
            throw new NotFoundException($"Event with id {eventId} not found", eventId);
        }

        if (eventDTO.EventName != null)
            e.EventName = new EventName(eventDTO.EventName);
        if (eventDTO.Description != null)
            e.Description = eventDTO.Description;
        if (eventDTO.Duration.HasValue)
            e.Duration = new Duration(eventDTO.Duration.Value);

        if (eventDTO.EventTagIds != null)
        {
            e.EventTagIds = eventDTO.EventTagIds.ToList();
        }
        else
        {
            e.EventTagIds = new List<Guid>();
        }

        if (eventDTO.ClassSize.HasValue)
            e.ClassSize = eventDTO.ClassSize.Value;

        if (eventDTO.Price.HasValue)
            e.Price = eventDTO.Price.Value;

        if (eventDTO.FormFile != null && eventDTO.FormFile.Length > 0)
        {
            if (!string.IsNullOrEmpty(e.ThumbnailUrl))
            {
                try
                {
                    await imageStorageService.DeleteImageAsync(e.ThumbnailUrl);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to delete previous image for event {EventName}", e.EventName);
                }
            }

            string thumbnailUrl;
            try
            {
                using (var stream = eventDTO.FormFile.OpenReadStream())
                {
                    if (ImageUtilities.IsSvg(stream))
                    {
                        if (!ImageUtilities.IsSvgAspectRatioValid(stream, EmailImage.RequiredAspectRatio))
                        {
                            throw new InvalidImageAspectRatioException("The uploaded image does not have the required 4:3 aspect ratio.");
                        }
                    }
                    else if (!ImageUtilities.IsEventImageAspectRatioValid(stream, EmailImage.RequiredAspectRatio))
                    {
                        throw new InvalidImageAspectRatioException("The uploaded image does not have the required 4:3 aspect ratio.");
                    }
                }
                var fileName = $"{e.EventName}_{e.Id}{Path.GetExtension(eventDTO.FormFile.FileName)}";
                thumbnailUrl = await imageStorageService.SaveImageAsync(eventDTO.FormFile, fileName);
                e.ThumbnailUrl = thumbnailUrl;
            }
            catch (InvalidImageAspectRatioException)
            {
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save image for event {EventName}", e.EventName);
                throw new InvalidOperationException("Failed to save event image", ex);
            }
        }

        await context.SaveChangesAsync();

        // Fetch EventTag data separately since we're now using EventTagIds
        var eventTags = await context.EventTags
            .Where(et => e.EventTagIds.Contains(et.Id))
            .Select(et => new EventTagDTO
            {
                Id = et.Id,
                Name = et.Name.Value
            })
            .ToArrayAsync();

        return new EventDTO
        {
            Id = e.Id,
            EventName = e.EventName.ToString(),
            Description = e.Description,

            Duration = e.Duration,
            ThumbnailUrl = e.ThumbnailUrl,
            EventTagIds = eventTags.Select(et => et.Id).ToArray(),
            ClassSize = e.ClassSize,
            Price = e.Price,
            Occurrences = e.Occurrences
                .Where(o => !o.isDeleted)
                .Select(o => new OccurrenceDTO
                {
                    Id = o.Id,
                    Attendees = o.Attendees.Select(a => new OccurrenceAttendeeDTO
                    {
                        Id = a.UserId,
                        FirstName = a.User?.FirstName ?? "",
                        LastName = a.User?.LastName ?? "",
                        Email = a.User?.Email?.Value ?? ""
                    }).ToList(),
                    EventId = o.EventId,
                    Leaders = o.Leaders.Select(l => new OccurrenceAttendeeDTO
                    {
                        Id = l.UserId,
                        FirstName = l.User?.FirstName ?? "",
                        LastName = l.User?.LastName ?? "",
                        Email = l.User?.Email?.Value ?? ""
                    }).ToList(),
                    ScheduleStart = DateTime.SpecifyKind(o.ScheduleStart?.Value ?? DateTime.MinValue, DateTimeKind.Utc),
                    Status = o.Status,
                    EventName = o.Event.EventName.Value,
                }).ToList()
        };

    }
}