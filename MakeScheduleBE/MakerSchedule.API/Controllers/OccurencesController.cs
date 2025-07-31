using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;


namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class OccurrencesController(IEventService eventService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OccurrenceDTO>> CreateOccuranceAsync([FromBody] CreateOccurrenceDTO createOccurrenceDTO)
    {
        var occurrence = await eventService.CreateOccurrenceAsync(createOccurrenceDTO);
        return Ok(occurrence);
    }

    [HttpPut]
    public async Task<ActionResult<OccurrenceDTO>> UpdateOccuranceAsync([FromBody] UpdateOccurrenceDTO updateOccurrenceDTO)
    {
        var occurrence = await eventService.UpdateOccuranceAsync(updateOccurrenceDTO);
        return Ok(occurrence);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOccuranceAsync(Guid id)
    {
        var isSuccess = await eventService.DeleteOccuranceAsync(id);
        return Ok(isSuccess);
    }

    [HttpGet()]
    public async Task<ActionResult> GetOccurancesByDateAsync([FromQuery] SearchOccurrenceDTO search)
    {
        var occurrences = await eventService.GetOccurancesByDateAsync(search);
        return Ok(occurrences);
    }   
}