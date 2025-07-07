using System.ComponentModel.Design;

using MakerSchedule.Application.DTOs.Event;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services
{
    public class EventService : IEventService
    {
        private readonly ILogger<EventService> _logger;
        private readonly ApplicationDbContext _dbContext;



        public EventService(ILogger<EventService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _dbContext = context;
        }
        public async Task<IEnumerable<EventListDTO>> GetAllEventsAsync()
        {
            try
            {
                var events = await _dbContext.Events.ToListAsync();

                var eventListDTO = events.Select(e => new EventListDTO
                {
                    Id = e.Id,
                    EventName = e.EventName,
                    ScheduleStart = ((DateTimeOffset)e.ScheduleStart).ToUnixTimeMilliseconds(),
                    Duration = e.Duration,
                    EventType = e.EventType,
                    Description = e.Description,

                }).ToList();

                return eventListDTO;

            }
            catch (BaseException ex)
            {
                _logger.LogError(ex, "Error fetching events");
                throw new BaseException("Failed to fetch events", "FETCH_ERROR", 500, ex);
            }
        }

        public async Task<EventDTO> GetEventAsync(int eventId)
        {
            try
            {
                var eventItem = await _dbContext.Events
                                .Include(e => e.Attendees)
                                .Include(e => e.Leaders)
                                .FirstOrDefaultAsync(e => e.Id == eventId);

                if (eventItem == null)
                {
                    throw new NotFoundException("Event", eventId);
                }

                var scheduleStart = ((DateTimeOffset)eventItem.ScheduleStart).ToUnixTimeMilliseconds();

                return new EventDTO
                {
                    Description = eventItem.Description,
                    EventName = eventItem.EventName,
                    Attendees = eventItem.Attendees.Select(e => e.Id).ToList(),
                    Id = eventId,
                    Leaders = eventItem.Leaders.Select(e => e.Id).ToList(),
                    ScheduleStart = scheduleStart,
                    Duration = eventItem.Duration,
                };

            }
            catch (BaseException ex)
            {
                _logger.LogError(ex, "Error fetching event");
                throw new BaseException("Failed to fetch event", "FETCH_ERROR", 500, ex);
            }
        }

        public async Task<int> CreateEventAsync(CreateEventDTO eventDTO)
        {
            try
            {

                var attendees = await _dbContext.Customers.Where(c => eventDTO.Attendees.Contains(c.Id)).ToListAsync();
                var leaders = await _dbContext.Employees.Where(e => eventDTO.Leaders.Contains(e.Id)).ToListAsync();
                var startDateTime = DateTimeOffset.FromUnixTimeMilliseconds(eventDTO.ScheduleStart).UtcDateTime;    

                var eventItem = new Event
                {
                    EventName = eventDTO.EventName,
                    Description = eventDTO.Description,
                    Attendees = attendees,
                    Leaders = leaders,
                    ScheduleStart = startDateTime,
                    Duration = eventDTO.Duration,
                    EventType = eventDTO.EventType
                };

                _dbContext.Events.Add(eventItem);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Successfully created event with ID: {EventId}", eventItem.Duration);

                return eventItem.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event");
                throw;
            }
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            var eventItem = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            if (eventItem == null)
            {
                _logger.LogWarning("Event with ID {eventId} not found.", eventId);
                throw new NotFoundException("Event not found", eventId);
            }

            _dbContext.Remove(eventItem);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return true;
             }
             return false;

        }
    }
}