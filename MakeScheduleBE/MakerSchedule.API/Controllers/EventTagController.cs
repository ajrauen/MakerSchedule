using MakerSchedule.Application.DTO.EventTag;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventTagController(IEventTagService eventTagService) : ControllerBase
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

    [HttpGet("{eventTagId}")]
    public async Task<ActionResult<EventTagDTO>> GetEventTagById(Guid eventTagId)
    {
        var eventTag = await eventTagService.GetEventTagByIdAsync(eventTagId);

        return Ok(eventTag);
    }

    [HttpPatch]
    public async Task<ActionResult<EventTagDTO>> PatchEventTag(Guid eventTagId, PatchEventTagDTO eventTagDto)
    {
        var updatedEventTag = await eventTagService.PatchEventTagAsync(eventTagId, eventTagDto);
        return Ok(updatedEventTag);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteEventTag(Guid eventTagId)
    {
        var result = await eventTagService.DeleteEventTagAsync(eventTagId);
        return Ok(eventTagId);
    }
}