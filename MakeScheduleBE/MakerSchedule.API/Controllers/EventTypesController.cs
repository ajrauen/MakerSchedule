
using Microsoft.AspNetCore.Mvc;
using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.DTO.EventType;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventTypesController(IEventTypeService eventTypeService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventTypeDTO>>> GetAllEventTypes()
    {
        var events = await eventTypeService.GetAllEventTypesAsync();
        return Ok(events);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateEventType([FromBody] string name)
    {
        var eventTypeDTO = new CreateEventTypeDTO { eventTypes = name };
        var eventId = await eventTypeService.CreateEventTypeAsync(eventTypeDTO);
        return CreatedAtAction(nameof(GetAllEventTypes), new { id = eventId }, eventId);
    }


    [HttpDelete("{eventId}")]
    public async Task<IActionResult> DeleteEvent(Guid eventId)
    {
        var deleted = await eventTypeService.DeleteEventTypeAsync(eventId);
        if (deleted)
        {
            return Ok(deleted);
        }
        return NotFound();
    }
}