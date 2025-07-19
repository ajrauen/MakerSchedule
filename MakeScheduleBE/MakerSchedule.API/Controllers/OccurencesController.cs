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
}