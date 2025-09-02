
using Microsoft.AspNetCore.Mvc;
using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.DTO.Occurrence;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventsController(IEventService _eventService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventListDTO>>> GetAllEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDTO>> GetEvent(Guid id)
    {
        var eventItem = await _eventService.GetEventAsync(id);

        return Ok(eventItem);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<EventDTO>> CreateEvent([FromForm] CreateEventDTO dto)
    {
        var eventDto = await _eventService.CreateEventAsync(dto);
        return Ok(eventDto);
    }

    [HttpPatch("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<EventDTO>> PathchEvent([FromForm] PatchEventDTO dto, Guid id)
    {
        var eventDto = await _eventService.PatchEventAsync(id, dto);
        return Ok(eventDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var deleted = await _eventService.DeleteEventAsync(id);
        if (deleted)
        {
            return Ok(deleted);
        }
        return NotFound();
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserOccurrence([FromBody] RegisterUserOccurrenceDTO registerDTO)
    {
        var result = await _eventService.RegisterUserForOccurrenceAsync(registerDTO);
        if (result)
        {
            return Ok();
        }
        return BadRequest();
    }
}