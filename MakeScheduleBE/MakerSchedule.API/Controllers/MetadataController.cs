using MakerSchedule.Application.DomainUsers.Queries;
using MakerSchedule.Application.DTO.Metadata;
using MakerSchedule.Application.Events.Queries;


using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MetadataController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    [Route("events")]
    public async Task<ActionResult<EventsMetadataDTO>> GetEventsMetadata()
    {
        var query = new GetEventMetaDataQuery();
        var metadata = await mediator.Send(query);
        return Ok(metadata);
    }

    [HttpGet]
    [Route("users")]
    public async Task<ActionResult<UserMetaDataDTO>> GetUsersMetadata()
    {
        var query = new GetUserMetaDataQuery();
        var metadata = await mediator.Send(query);
        return Ok(metadata);
    }

}