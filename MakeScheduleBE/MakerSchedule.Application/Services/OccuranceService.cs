using MakerSchedule.Application.DTOs.Occurence;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.Event;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class OccurrenceService : IOccurrenceService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<OccurrenceService> _logger;

    public OccurrenceService(IApplicationDbContext context, ILogger<OccurrenceService> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public async Task<OccurenceDTO> GetOccurrenceByIdAsync(int id)
    {

        var occurence = await _dbContext.Occurrences.Where(occurance => occurance.Id == id).FirstOrDefaultAsync();
        if (occurence == null) throw new NotFoundException("Occurence with ${OccurenceId} was not found", id);

        return new OccurenceDTO()
        {
            Attendees = occurence.Attendees,
            Id = occurence.Id,
            EventId = occurence.EventId,
            Leaders = occurence.Leaders,
            Duration = occurence.Duration,
        };
    }

    public async Task<IEnumerable<OccurenceListDTO>> GetAllOccurrencesAsync()
    {
        return await _dbContext.Occurrences.Select(o => new OccurenceListDTO
        {
            Id = o.Id,
        }).ToListAsync();
    }

    public async Task<int> CreateOccurrenceAsync(CreateOccurenceDTO occurenceDTO)
    {
        var attendees = await _dbContext.Customers.Where(customer => occurenceDTO.Attendees.Contains(customer.Id)).Select(customer => customer.Id).ToListAsync();
        var leaders = await _dbContext.Employees.Where(employee => occurenceDTO.Leaders.Contains(employee.Id)).Select(employee => employee.Id).ToListAsync();
        var scheduledStart = DateTimeOffset.FromUnixTimeMilliseconds(occurenceDTO.ScheduleStart).UtcDateTime;

        // Load the Event aggregate root
        var eventEntity = await _dbContext.Events.Include(e => e.Occurrences).FirstOrDefaultAsync(e => e.Id == occurenceDTO.EventId);
        if (eventEntity == null)
            throw new NotFoundException($"Event with id {occurenceDTO.EventId} not found", occurenceDTO.EventId);

        var info = new OccurrenceInfo(scheduledStart, occurenceDTO.Duration, attendees, leaders);
        var newOccurrence = eventEntity.AddOccurrence(info);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Successfully created occurrence with {OccurrenceId}", newOccurrence.Id);
        return newOccurrence.Id;
    }
}