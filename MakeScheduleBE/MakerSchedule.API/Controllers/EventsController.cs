
using Microsoft.AspNetCore.Mvc;
using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.Interfaces;

namespace MakerSchedule.API.Controllers;

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
    public async Task<ActionResult<IEnumerable<EventListDTO>>> GetAllEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDTO>> GetEvent(string id)
    {
        var eventItem = await _eventService.GetEventAsync(id);
        return Ok(eventItem);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateEvent([FromForm] CreateEventDTO dto)
    {
        var eventId = await _eventService.CreateEventAsync(dto);
        return Ok(eventId);
    }

    [HttpDelete("${eventId}")]
    public async Task<IActionResult> DeleteEvent(string eventId)
    {
        var deleted = await _eventService.DeleteEventAsync(eventId);
        if (deleted)
        {
            return Ok(deleted);
        }
        return NotFound();
    }
}