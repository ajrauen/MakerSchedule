
using Microsoft.AspNetCore.Mvc;
using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Events.Queries;
using MediatR;
using MakerSchedule.Application.Events.Commands;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventsController( IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventListDTO>>> GetAllEvents()
    {
        var query = new GetEventsQuery();
        var events = await mediator.Send(query);
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDTO>> GetEvent(Guid id)
    {
        var query = new GetEventByIdQuery(id);
        var evt = await mediator.Send(query);
        return Ok(evt);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<EventDTO>> CreateEvent([FromForm] CreateEventDTO dto)
    {
        var command = new CreateEventCommand(dto);
        var eventDto = await mediator.Send(command);
        return Ok(eventDto);
    }

    [HttpPatch("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<EventDTO>> PathchEvent([FromForm] PatchEventDTO dto, Guid id)
    {
        var command = new PatchEventCommand(id, dto);
        var eventDto = await mediator.Send(command);
        return Ok(eventDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var command = new DeleteEventCommand(id);
        var deleted = await mediator.Send(command);
        if (deleted)
        {
            return Ok(deleted);
        }
        return NotFound();
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserOccurrence([FromBody] RegisterUserOccurrenceDTO registerDTO)
    {
        var command = new RegisterUserForOccurrenceCommand(registerDTO);
        var result = await mediator.Send(command);
        if (result)
        {
            return Ok();
        }
        return BadRequest();
    }
}