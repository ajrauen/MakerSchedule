using System.ComponentModel.Design;

using MakerSchedule.Application.DTOs.Event;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
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
                var eventItem = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId);

                if (eventItem == null)
                {
                    throw new NotFoundException("Event", eventId);
                }

                return new EventDTO
                {
                    Description = eventItem.Description,
                    EventName = eventItem.EventName,
                    Attendees = eventItem.Attendees,
                    Id = eventId,
                    Leaders = eventItem.Leaders,
                    ScheduleStart = eventItem.ScheduleStart,
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
                var eventItem = new Event
                {
                    EventName = eventDTO.EventName,
                    Description = eventDTO.Description,
                    Attendees = eventDTO.Attendees,
                    Leaders = eventDTO.Leaders,
                    ScheduleStart = eventDTO.ScheduleStart
                };

                _dbContext.Events.Add(eventItem);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Successfully created event with ID: {EventId}", eventItem.Id);

                return eventItem.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event");
                throw; 
            }
        }
    }
}