
using MakerSchedule.Application.Services;
using MakerSchedule.Domain.Entities;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private ILogger<EventsController> _logger;

        public EventsController(IEventService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _eventService.GetEventAsync(id);
            return Ok(eventItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO dto)
        {
            var eventId = await _eventService.CreateEventAsync(dto);
            return Ok(eventId);
        }

        [HttpDelete("${eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var deleted = await _eventService.DeleteEventAsync(eventId);
            if (deleted)
            {
                return Ok(deleted);
            }
            return NotFound();
        }


    }
}