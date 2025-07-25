using System.ComponentModel.Design;

using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Enums;
using MakerSchedule.Domain.Exceptions;
using MakerSchedule.Domain.Utilties.ImageUtilities;
using MakerSchedule.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class EventService(IApplicationDbContext context, ILogger<EventService> logger, IImageStorageService imageStorageService) : IEventService
{
    private readonly IApplicationDbContext _context = context;
    private readonly ILogger<EventService> _logger = logger;
    private readonly IImageStorageService _imageStorageService = imageStorageService;
    private const double RequiredAspectRatio = 4.0 / 3.0;



    public async Task<IEnumerable<EventListDTO>> GetAllEventsAsync()
    {



        return await _context.Events.Select(e => new EventListDTO
        {
            Id = e.Id,
            EventName = e.EventName.ToString(),
            Description = e.Description,
            EventType = e.EventType.ToString(),
            Duration = e.Duration,
            ThumbnailUrl = e.ThumbnailUrl,
        }).ToListAsync();
    }

    public async Task<EventDTO> GetEventAsync(Guid id)
    {
        var e = await _context.Events
        .Include(ev => ev.Occurrences)
            .ThenInclude(ev => ev.Attendees)
                .ThenInclude(a => a.User)
                    .ThenInclude(u => u.User)
        .Include(ev => ev.Occurrences)
            .ThenInclude(ev => ev.Leaders)
                .ThenInclude(l => l.User)
                    .ThenInclude(u => u.User)
        .FirstOrDefaultAsync(ev => ev.Id == id);

        if (e == null) throw new NotFoundException("Event", id);

        var eventType  = EventTypeEnum.Unknown;
        if (Enum.TryParse<EventTypeEnum>(e.EventType.ToString(), out var eventTypeEnum))
        {
            eventType = eventTypeEnum;
        }
        else
        {
            throw new ArgumentException($"Invalid event type: {e.EventType.ToString()}", nameof(e.EventType));
        }

        return new EventDTO
        {
            Id = e.Id,
            EventName = e.EventName.ToString(),
            Description = e.Description,
            EventType = eventType.ToString(),
            Duration = e.Duration,
            ThumbnailUrl = e.ThumbnailUrl,
            Occurences = e.Occurrences
                .Where(o => !o.isDeleted)
                .Select(o => new OccurenceDTO
                {
                    Id = o.Id,
                    Attendees = o.Attendees.Select(a => new OccurrenceUserDTO
                    {
                        Id = a.UserId.ToString(),
                        FirstName = a.User?.FirstName ?? "",
                        LastName = a.User?.LastName ?? ""
                    }).ToList(),
                    Duration = o.Duration,
                    EventId = o.EventId,
                    Leaders = o.Leaders.Select(l => new OccurrenceUserDTO
                    {
                        Id = l.UserId.ToString(),
                        FirstName = l.User?.FirstName ?? "",
                        LastName = l.User?.LastName ?? ""
                    }).ToList(),
                    ScheduleStart = DateTime.SpecifyKind(o.ScheduleStart.Value, DateTimeKind.Utc),
                    Status = o.Status
                })
        };
    }

    public async Task<Guid> CreateEventAsync(CreateEventDTO dto)
    {

        if (dto.FormFile == null || dto.FormFile.Length == 0)
        {
            throw new ArgumentException("Image file is required for event creation", nameof(dto.FormFile));
        }

        var eventType  = EventTypeEnum.Unknown;
        if (Enum.TryParse<MakerSchedule.Domain.Enums.EventTypeEnum>(dto.EventType, out var eventTypeEnum))
        {
            eventType = eventTypeEnum;
        }
        else
        {
            throw new ArgumentException($"Invalid event type: {dto.EventType}", nameof(dto.EventType));
        }

        var e = new Event
        {
            EventName = new EventName(dto.EventName),
            Description = dto.Description,
            EventType = eventType,
            Duration = dto.Duration > 0 ? new Duration(dto.Duration) : null,

        };
        _context.Events.Add(e);
        await _context.SaveChangesAsync();


        string thumbnailUrl;
        try
        {

            using (var stream = dto.FormFile.OpenReadStream())
            {
                if (ImageUtilities.IsSvg(stream))
                {
                    if (!ImageUtilities.IsSvgAspectRatioValid(stream, RequiredAspectRatio))
                    {
                        throw new InvalidImageAspectRatioException("The uploaded image does not have the required 4:3 aspect ratio.");
                    }
                }
                else if (!ImageUtilities.IsEventImageAspectRatioValid(stream, RequiredAspectRatio))
                {
                    throw new InvalidImageAspectRatioException("The uploaded image does not have the required 4:3 aspect ratio.");
                }
            }

            var fileName = $"{dto.EventName}_{e.Id}{Path.GetExtension(dto.FormFile.FileName)}";
            thumbnailUrl = await _imageStorageService.SaveImageAsync(dto.FormFile, fileName);
            e.ThumbnailUrl = thumbnailUrl;
            await _context.SaveChangesAsync();
        }
        catch (InvalidImageAspectRatioException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save image for event {EventName}", dto.EventName);
            _context.Events.Remove(e);
            await _context.SaveChangesAsync();
            throw new InvalidOperationException("Failed to save event image", ex);
        }

        return e.Id;
    }


    public async Task<Guid> PatchEventAsync(Guid eventId, PatchEventDTO dto)
    {
        var e = await _context.Events.FindAsync(eventId);
        if (e == null)
        {
            throw new NotFoundException($"Event with id {eventId} not found", eventId);
        }

        if (dto.EventName != null)
            e.EventName = new EventName(dto.EventName);
        if (dto.Description != null)
            e.Description = dto.Description;
        if (dto.Duration.HasValue)
            e.Duration = new Duration(dto.Duration.Value);
        if (dto.EventType != null)
        {
            if (Enum.TryParse<MakerSchedule.Domain.Enums.EventTypeEnum>(dto.EventType, out var eventTypeEnum))
            {
                e.EventType = eventTypeEnum;
            }
            else
            {
                throw new ArgumentException($"Invalid event type: {dto.EventType}", nameof(dto.EventType));
            }
        }
        if (dto.FormFile != null && dto.FormFile.Length > 0)
        {
            if (!string.IsNullOrEmpty(e.ThumbnailUrl))
            {
                try
                {
                    await _imageStorageService.DeleteImageAsync(e.ThumbnailUrl);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete previous image for event {EventName}", e.EventName);
                }
            }

            string thumbnailUrl;
            try
            {
                using (var stream = dto.FormFile.OpenReadStream())
                {
                    if (ImageUtilities.IsSvg(stream))
                    {
                        if (!ImageUtilities.IsSvgAspectRatioValid(stream, RequiredAspectRatio))
                        {
                            throw new InvalidImageAspectRatioException("The uploaded image does not have the required 4:3 aspect ratio.");
                        }
                    }
                    else if (!ImageUtilities.IsEventImageAspectRatioValid(stream, RequiredAspectRatio))
                    {
                        throw new InvalidImageAspectRatioException("The uploaded image does not have the required 4:3 aspect ratio.");
                    }
                }
                var fileName = $"{e.EventName}_{e.Id}{Path.GetExtension(dto.FormFile.FileName)}";
                thumbnailUrl = await _imageStorageService.SaveImageAsync(dto.FormFile, fileName);
                e.ThumbnailUrl = thumbnailUrl;
            }
            catch (InvalidImageAspectRatioException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save image for event {EventName}", e.EventName);
                throw new InvalidOperationException("Failed to save event image", ex);
            }
        }

        await _context.SaveChangesAsync();
        return e.Id;
    }



    public async Task<bool> DeleteEventAsync(Guid id)
    {
        var e = await _context.Events.FindAsync(id);
        if (e == null) return false;
        _context.Events.Remove(e);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Guid> CreateOccurrenceAsync(CreateOccurenceDTO occurrenceDTO)
    {

        var eventEntity = await _context.Events.Include(e => e.Occurrences).FirstOrDefaultAsync(e => e.Id == occurrenceDTO.EventId);
        if (eventEntity == null)
            throw new NotFoundException($"Event with id {occurrenceDTO.EventId} not found", occurrenceDTO.EventId);

        OccurrenceInfo info;
        Occurrence newOccurrence;
        try
        {
            var start = occurrenceDTO.ScheduleStart;
            if (start.Kind == DateTimeKind.Local)
                start = start.ToUniversalTime();
            else if (start.Kind == DateTimeKind.Unspecified)
                start = DateTime.SpecifyKind(start, DateTimeKind.Local).ToUniversalTime();
            // Now start is always UTC
            info = new OccurrenceInfo(start, occurrenceDTO.Duration);
            newOccurrence = eventEntity.AddOccurrence(info);
            _context.Occurrences.Add(newOccurrence);
        }
        catch (ScheduleDateOutOfBoundsException ex)
        {
            _logger.LogError("Exception type: {Type}, message: {Message}", ex.GetType().FullName, ex.Message);
            throw new BaseException(ex.Message, "SCHEDULE_START_INVALID", 400);
        }


        await _context.SaveChangesAsync();

        var uniqueLeaderIds = occurrenceDTO.Leaders.Distinct().ToList();
        foreach (var leaderId in uniqueLeaderIds)
        {
            var leader = await _context.DomainUsers.FindAsync(leaderId);
            if (leader != null)
            {
                var occurrenceLeader = new OccurrenceLeader
                {
                    OccurrenceId = newOccurrence.Id,
                    UserId = leaderId,
                    AssignedAt = DateTime.UtcNow
                };
                _context.OccurrenceLeaders.Add(occurrenceLeader);
            }
        }

        foreach (var attendeeId in occurrenceDTO.Attendees)
        {
            var attendee = await _context.DomainUsers.FindAsync(attendeeId);
            if (attendee != null)
            {
                var occurrenceAttendee = new OccurrenceAttendee
                {
                    OccurrenceId = newOccurrence.Id,
                    UserId = attendeeId,
                    RegisteredAt = DateTime.UtcNow
                };
                _context.OccurrenceAttendees.Add(occurrenceAttendee);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully created occurrence with {OccurrenceId}", newOccurrence.Id);
        return newOccurrence.Id;
    }

    public async Task<bool> UpdateOccuranceAsync(UpdateOccurenceDTO occurrenceDTO)
    {


        var eventEntity = await _context.Events.Include(e => e.Occurrences).FirstOrDefaultAsync(e => e.Id == occurrenceDTO.EventId);
        if (eventEntity == null)
            throw new NotFoundException($"Event with id {occurrenceDTO.EventId} not found", occurrenceDTO.EventId);

        var occurrence = await _context.Occurrences.FirstOrDefaultAsync(o => o.Id == occurrenceDTO.Id && o.EventId == occurrenceDTO.EventId);
        if (occurrence == null)
            throw new NotFoundException($"Occurrence with id {occurrenceDTO.Id} not found", occurrenceDTO.Id);

        var start = occurrenceDTO.ScheduleStart;
        if (start.Kind == DateTimeKind.Local)
            start = start.ToUniversalTime();
        else if (start.Kind == DateTimeKind.Unspecified)
            start = DateTime.SpecifyKind(start, DateTimeKind.Local).ToUniversalTime();
        occurrence.ScheduleStart = ScheduleStart.Create(start);
        occurrence.Duration = occurrenceDTO.Duration;

        var existingLeaders = _context.OccurrenceLeaders.Where(l => l.OccurrenceId == occurrence.Id);
        _context.OccurrenceLeaders.RemoveRange(existingLeaders);
        var existingAttendees = _context.OccurrenceAttendees.Where(a => a.OccurrenceId == occurrence.Id);
        _context.OccurrenceAttendees.RemoveRange(existingAttendees);

        var uniqueLeaderIds = occurrenceDTO.Leaders.Distinct().ToList();
        foreach (var leaderId in uniqueLeaderIds)
        {
            var leader = await _context.DomainUsers.FindAsync(leaderId);
            if (leader != null)
            {
                var occurrenceLeader = new OccurrenceLeader
                {
                    OccurrenceId = occurrence.Id,
                    UserId = leaderId,
                    AssignedAt = DateTime.UtcNow
                };
                _context.OccurrenceLeaders.Add(occurrenceLeader);
            }
        }

        foreach (var attendeeId in occurrenceDTO.Attendees.Distinct())
        {
            var attendee = await _context.DomainUsers.FindAsync(attendeeId);
            if (attendee != null)
            {
                var occurrenceAttendee = new OccurrenceAttendee
                {
                    OccurrenceId = occurrence.Id,
                    UserId = attendeeId,
                    RegisteredAt = DateTime.UtcNow
                };
                _context.OccurrenceAttendees.Add(occurrenceAttendee);
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteOccuranceAsync(Guid occurrenceid)
    {
        var occurrence = await _context.Occurrences.FirstOrDefaultAsync(o => o.Id == occurrenceid);
        if (occurrence == null)
            throw new NotFoundException($"Occurrence with id {occurrenceid} not found", occurrenceid);
            occurrence.isDeleted = true;
        _context.Occurrences.Update(occurrence);

        await _context.SaveChangesAsync();
        return true;
    }


}