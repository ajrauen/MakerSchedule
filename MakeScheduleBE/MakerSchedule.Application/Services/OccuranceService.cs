using MakerSchedule.Application.DTOs.Occurence;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class OccurrenceService : IOccurrenceService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<OccurrenceService> _logger;

    public OccurrenceService(ApplicationDbContext context, ILogger<OccurrenceService> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public async Task<OccurenceDTO> GetOccurrenceByIdAsync(int id)
    {

        var occurence = await _dbContext.Occurences.Where(occurance => occurance.Id == id).FirstOrDefaultAsync();
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
        return await _dbContext.Occurences.Select(o => new OccurenceListDTO
        {
            Id = o.Id,
        }).ToListAsync();
    }

    public async Task<int> CreateOccurrenceAsync(CreateOccurenceDTO occurenceDTO)
    {

        var attendees = await _dbContext.Customers.Where(customer => occurenceDTO.Attendees.Contains(customer.Id)).Select(customer => customer.Id).ToListAsync();
        var leaders = await _dbContext.Employees.Where(employee => occurenceDTO.Leaders.Contains(employee.Id)).Select(employee => employee.Id).ToListAsync();

        var scheduledStart = DateTimeOffset.FromUnixTimeMilliseconds(occurenceDTO.ScheduleStart).UtcDateTime;

        var newOccurence = new Occurrence()
        {
            Attendees = attendees,
            Leaders = leaders,
            ScheduleStart = scheduledStart,
            EventId = occurenceDTO.EventId,
            Duration = occurenceDTO.Duration,
        };
        _dbContext.Occurences.Add(newOccurence);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Successfully created event with ${OccurenceId}", newOccurence.Id);
        return newOccurence.Id;
        
    }
}