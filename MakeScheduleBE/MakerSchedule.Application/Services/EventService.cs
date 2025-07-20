using System.ComponentModel.Design;

using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.Event;
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
            EventType = e.EventType,
            Duration = e.Duration,
            FileUrl = e.FileUrl,
        }).ToListAsync();
    }

    public async Task<EventDTO> GetEventAsync(Guid id)
    {
        var e = await _context.Events
        .Include(ev => ev.Occurrences)
            .ThenInclude(ev => ev.Attendees)
        .Include(ev => ev.Occurrences)
            .ThenInclude(ev => ev.Leaders)
        .FirstOrDefaultAsync(ev => ev.Id == id);

        if (e == null) throw new NotFoundException("Event", id);
        return new EventDTO
        {
            Id = e.Id,
            EventName = e.EventName.ToString(),
            Description = e.Description,
            EventType = e.EventType,
            Duration = e.Duration,
            FileUrl = e.FileUrl,
            Occurences = e.Occurrences.Select(o => new OccurenceDTO
            {
                Id = o.Id,
                Attendees = o.Attendees.Select(a => a.Id.ToString()).ToList(),
                Duration = o.Duration,
                EventId = o.EventId,
                Leaders = o.Leaders.Select(a => a.UserId.ToString()).ToList(),
                ScheduleStart = o.ScheduleStart != null ? new DateTimeOffset(o.ScheduleStart.Value).ToString("O") : string.Empty,
            })
        };
    }

    public async Task<Guid> CreateEventAsync(CreateEventDTO dto)
    {

        if (dto.FormFile == null || dto.FormFile.Length == 0)
        {
            throw new ArgumentException("Image file is required for event creation", nameof(dto.FormFile));
        }

        var e = new Event
        {
            EventName = new EventName(dto.EventName),
            Description = dto.Description,
            EventType = dto.EventType,
            Duration = dto.Duration > 0 ? new Duration(dto.Duration) : null,

        };
        _context.Events.Add(e);
        await _context.SaveChangesAsync();


        string fileUrl;
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
            fileUrl = await _imageStorageService.SaveImageAsync(dto.FormFile, fileName);
            e.FileUrl = fileUrl;
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

        public async Task<Guid> UpdateEventAsync(Guid eventId, UpdateEventDTO dto)
    {

        if (dto.FormFile == null || dto.FormFile.Length == 0)
        {
            throw new ArgumentException("Image file is required for event creation", nameof(dto.FormFile));
        }

        var e = _context.Events.FirstOrDefault(e => e.Id == eventId);

        if (e == null)
        {
            throw new NotFoundException($"Event with id {eventId} not found", eventId);
        }


        
        string fileUrl;
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
            fileUrl = await _imageStorageService.SaveImageAsync(dto.FormFile, fileName);
            e.FileUrl = fileUrl;
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
        if (dto.EventType.HasValue)
            e.EventType = dto.EventType.Value;
        if (dto.FormFile != null && dto.FormFile.Length > 0)
        {
            if (!string.IsNullOrEmpty(e.FileUrl))
            {
                try
                {
                    await _imageStorageService.DeleteImageAsync(e.FileUrl);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete previous image for event {EventName}", e.EventName);
                }
            }

            string fileUrl;
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
                fileUrl = await _imageStorageService.SaveImageAsync(dto.FormFile, fileName);
                e.FileUrl = fileUrl;
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
        var scheduledStart = DateTimeOffset.Parse(occurrenceDTO.ScheduleStart).UtcDateTime;

        // Load the Event aggregate root
        var eventEntity = await _context.Events.Include(e => e.Occurrences).FirstOrDefaultAsync(e => e.Id == occurrenceDTO.EventId);
        if (eventEntity == null)
            throw new NotFoundException($"Event with id {occurrenceDTO.EventId} not found", occurrenceDTO.EventId);

        OccurrenceInfo info;
        Occurrence newOccurrence;
        try
        {
            info = new OccurrenceInfo(scheduledStart, occurrenceDTO.Duration);
            newOccurrence = eventEntity.AddOccurrence(info);
            _context.Occurrences.Add(newOccurrence);
        }
        catch (ScheduleDateOutOfBoundsException ex)
        {
            _logger.LogError("Exception type: {Type}, message: {Message}", ex.GetType().FullName, ex.Message);
            throw new BaseException(ex.Message, "SCHEDULE_START_INVALID", 400);
        }
        

        await _context.SaveChangesAsync();

        foreach (var leaderId in occurrenceDTO.Leaders)
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


}