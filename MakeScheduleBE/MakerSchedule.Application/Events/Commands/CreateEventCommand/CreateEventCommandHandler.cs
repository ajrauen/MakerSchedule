using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.DTO.EventTag;
using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Utilties.ImageUtilities;
using MakerSchedule.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

using MediatR;

using Microsoft.Extensions.Logging;
using MakerSchedule.Application.Constants;
namespace MakerSchedule.Application.Events.Commands;

public class CreateEventCommandHandler(IApplicationDbContext context, IImageStorageService imageStorageService, ILogger<CreateEventCommandHandler> logger) : IRequestHandler<CreateEventCommand, EventDTO>
{
    public async Task<EventDTO> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var createEvent = request.CreateEventDTO;
        if (createEvent.FormFile == null || createEvent.FormFile.Length == 0)
        {
            throw new ArgumentException("Image file is required for event creation", nameof(createEvent.FormFile));
        }


        var e = new Event
        {
            EventName = new EventName(createEvent.EventName),
            Description = createEvent.Description,
            Duration = createEvent.Duration > 0 ? new Duration(createEvent.Duration) : null,
            ClassSize = createEvent.ClassSize,
            EventTagIds = createEvent.EventTagIds?.ToList() ?? new List<Guid>(),
            Price = createEvent.Price
        };
        e.EventTagIds = createEvent.EventTagIds?.ToList() ?? new List<Guid>();
        context.Events.Add(e);
        await context.SaveChangesAsync();


        string thumbnailUrl;
        try
        {

            using (var stream = createEvent.FormFile.OpenReadStream())
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

            var fileName = $"{createEvent.EventName}_{e.Id}{Path.GetExtension(createEvent.FormFile.FileName)}";
            thumbnailUrl = await imageStorageService.SaveImageAsync(createEvent.FormFile, fileName);
            e.ThumbnailUrl = thumbnailUrl;
            await context.SaveChangesAsync();
        }
        catch (InvalidImageAspectRatioException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save image for event {EventName}", createEvent.EventName);
            context.Events.Remove(e);
            await context.SaveChangesAsync();
            throw new InvalidOperationException("Failed to save event image", ex);
        }

        // Fetch EventTag data for the return DTO
        var eventTags = await context.EventTags
            .Where(et => e.EventTagIds.Contains(et.Id))
            .Select(et => new EventTagDTO
            {
                Id = et.Id,
                Name = et.Name.Value
            })
            .ToListAsync();

        return new EventDTO
        {
            Id = e.Id,
            EventName = e.EventName.ToString(),
            Description = e.Description,
            EventTagIds = eventTags.Select(et => et.Id).ToArray(),
            Duration = e.Duration,
            ThumbnailUrl = e.ThumbnailUrl,
            ClassSize = e.ClassSize,
            Price = e.Price,
            Occurrences = new List<OccurrenceDTO>()
        };
    }
}
