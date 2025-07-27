
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
    public async Task<ActionResult<Guid>> CreateEventType([FromBody] CreateEventTypeDTO createEventTypeDTO)
    {
        var eventTypeId = await eventTypeService.CreateEventTypeAsync(createEventTypeDTO);
        return Ok(eventTypeId);
    }


    [HttpDelete("{eventTypeId}")]
    public async Task<IActionResult> DeleteEvent(Guid eventTypeId)
    {
        var deleted = await eventTypeService.DeleteEventTypeAsync(eventTypeId);
        if (deleted)
        {
            return Ok(deleted);
        }
        return NotFound();
    }
}