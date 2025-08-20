using MakerSchedule.Application.DTO.EventTag;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventTagsController(IEventTagService eventTagService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<EventTagDTO>> CreateEventTag(CreateEventTagDTO createTagDTO)
    {
        var eventTag = await eventTagService.CreateEventTagAsync(createTagDTO);
        return Ok(eventTag);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventTagDTO>>> GetEventTags()
    {
        var eventTags = await eventTagService.GetEventTagAsync();
        return Ok(eventTags);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventTagDTO>> GetEventTagById(Guid id)
    {
        var eventTag = await eventTagService.GetEventTagByIdAsync(id);

        return Ok(eventTag);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<EventTagDTO>> PatchEventTag(Guid id, [FromBody] PatchEventTagDTO eventTagDto)
    {
        var updatedEventTag = await eventTagService.PatchEventTagAsync(id, eventTagDto);
        return Ok(updatedEventTag);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> DeleteEventTag(Guid id)
    {
        var result = await eventTagService.DeleteEventTagAsync(id);
        return Ok(id);
    }
}