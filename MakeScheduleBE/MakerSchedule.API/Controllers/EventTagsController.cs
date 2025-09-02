using MakerSchedule.Application.DTO.EventTag;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;
using MediatR;
using MakerSchedule.Application.EventTags.Queries;
using MakerSchedule.Application.EventTags.Commands;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventTagsController( IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<EventTagDTO>> CreateEventTag(CreateEventTagDTO createTagDTO)
    {
        var command = new CreateEventTagCommand(createTagDTO);
        var eventTag = await mediator.Send(command);
        return Ok(eventTag);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventTagDTO>>> GetEventTags()
    {
        var query = new GetEventTagsQuery();
        var eventTags = await mediator.Send(query);
        return Ok(eventTags);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventTagDTO>> GetEventTagById(Guid id)
    {
        var query = new GetEventTagByIdQuery(id);
        var eventTag = await mediator.Send(query);

        return Ok(eventTag);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<EventTagDTO>> PatchEventTag(Guid id, [FromBody] PatchEventTagDTO eventTagDto)
    {
        var command = new UpdateEventTagCommand(id, eventTagDto);
        var updatedEventTag = await mediator.Send(command);
        return Ok(updatedEventTag);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> DeleteEventTag(Guid id)
    {
        var command = new DeleteEventTagCommand(id);
        var result = await mediator.Send(command);
        return Ok(id);
    }
}