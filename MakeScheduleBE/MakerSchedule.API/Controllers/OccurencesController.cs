using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OccurrencesController(IEventService eventService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<string>> CreateOccuranceAsync([FromBody] CreateOccurenceDTO createOccurenceDTO)
    {
        var occurrence = await eventService.CreateOccurrenceAsync(createOccurenceDTO);
        return Ok(occurrence);
    }

    [HttpPut]
    public async Task<ActionResult<string>> UpdateOccuranceAsync([FromBody] UpdateOccurenceDTO updateOccurenceDTO)
    {
        var isSuccess = await eventService.UpdateOccuranceAsync(updateOccurenceDTO);
        return Ok(isSuccess);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOccuranceAsync(Guid id)
    {
        var isSuccess = await eventService.DeleteOccuranceAsync(id);
        return Ok(isSuccess);
    }
}

    // [HttpGet("{id}")]
    // public async Task<ActionResult<EventDTO>> GetEvent(Guid id)