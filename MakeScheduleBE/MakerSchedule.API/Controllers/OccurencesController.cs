using MakerSchedule.Application.DTO.Occurrence;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using MakerSchedule.Application.Events.Queries;
using MediatR;
using MakerSchedule.Application.Events.Commands;


namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class OccurrencesController( IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OccurrenceDTO>> CreateOccuranceAsync([FromBody] CreateOccurrenceDTO createOccurrenceDTO)
    {
        var command = new CreateOccurrenceCommand(createOccurrenceDTO);
        var occurrence = await mediator.Send(command);
        return Ok(occurrence);
    }

    [HttpPut]
    public async Task<ActionResult<OccurrenceDTO>> UpdateOccuranceAsync([FromBody] UpdateOccurrenceDTO updateOccurrenceDTO)
    {
        var command = new UpdateOccurrenceCommand(updateOccurrenceDTO);
        var occurrence = await mediator.Send(command);
        return Ok(occurrence);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOccuranceAsync(Guid id)
    {
        var command = new DeleteOccurrenceCommand(id);
        var isSuccess = await mediator.Send(command);
        return Ok(isSuccess);
    }

    [HttpGet()]
    public async Task<ActionResult> GetOccurancesByDateAsync([FromQuery] SearchOccurrenceDTO search)
    {
        var command = new GetOccurrencesByDateQuery(search);
        var occurrences = await mediator.Send(command);
        return Ok(occurrences);
    }   
}