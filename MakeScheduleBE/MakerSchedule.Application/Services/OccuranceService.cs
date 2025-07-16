using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Exceptions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class OccurrenceService(IApplicationDbContext context, ILogger<OccurrenceService> logger) : IOccurrenceService
{
    private readonly IApplicationDbContext _dbContext = context;
    private readonly ILogger<OccurrenceService> _logger = logger;

    public async Task<OccurenceDTO> GetOccurrenceByIdAsync(string id)
    {
        var occurrence = await _dbContext.Occurrences
            .Include(o => o.Leaders)
                .ThenInclude(ol => ol.User)
            .Include(o => o.Attendees)
                .ThenInclude(oa => oa.User)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (occurrence == null) 
            throw new NotFoundException($"Occurrence with id {id} was not found", id);

        return new OccurenceDTO()
        {
            Id = occurrence.Id,
            EventId = occurrence.EventId,
            Duration = occurrence.Duration,
            Attendees = occurrence.Attendees.Select(oa => oa.UserId).ToList(),
            Leaders = occurrence.Leaders.Select(ol => ol.UserId).ToList(),
        };
    }

    public async Task<IEnumerable<OccurenceListDTO>> GetAllOccurrencesAsync()
    {
        return await _dbContext.Occurrences.Select(o => new OccurenceListDTO
        {
            Id = o.Id,
        }).ToListAsync();
    }

    public async Task<string> CreateOccurrenceAsync(CreateOccurenceDTO occurrenceDTO)
    {
        var scheduledStart = DateTimeOffset.FromUnixTimeMilliseconds(occurrenceDTO.ScheduleStart).UtcDateTime;

        // Load the Event aggregate root
        var eventEntity = await _dbContext.Events.Include(e => e.Occurrences).FirstOrDefaultAsync(e => e.Id == occurrenceDTO.EventId);
        if (eventEntity == null)
            throw new NotFoundException($"Event with id {occurrenceDTO.EventId} not found", occurrenceDTO.EventId);

        OccurrenceInfo info;
        Occurrence newOccurrence;
        try
        {
            info = new OccurrenceInfo(scheduledStart, occurrenceDTO.Duration);
             newOccurrence = eventEntity.AddOccurrence(info);
        }
        catch (ScheduleDateOutOfBoundsException ex)
        {
            _logger.LogError("Exception type: {Type}, message: {Message}", ex.GetType().FullName, ex.Message);
            throw new BaseException(ex.Message, "SCHEDULE_START_INVALID", 400);
        }
        

        foreach (var leaderId in occurrenceDTO.Leaders)
        {
            var leader = await _dbContext.DomainUsers.FindAsync(leaderId);
            if (leader != null)
            {
                var occurrenceLeader = new OccurrenceLeader
                {
                    OccurrenceId = newOccurrence.Id,
                    UserId = leaderId,
                    AssignedAt = DateTime.UtcNow
                };
                _dbContext.OccurrenceLeaders.Add(occurrenceLeader);
            }
        }

        foreach (var attendeeId in occurrenceDTO.Attendees)
        {
            var attendee = await _dbContext.DomainUsers.FindAsync(attendeeId);
            if (attendee != null)
            {
                var occurrenceAttendee = new OccurrenceAttendee
                {
                    OccurrenceId = newOccurrence.Id,
                    UserId = attendeeId,
                    RegisteredAt = DateTime.UtcNow
                };
                _dbContext.OccurrenceAttendees.Add(occurrenceAttendee);
            }
        }

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Successfully created occurrence with {OccurrenceId}", newOccurrence.Id);
        return newOccurrence.Id;
    }
}