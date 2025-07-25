using MakerSchedule.Application.DTO.Metadata;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.Services;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MetadataController(MetadataService metadataService) : ControllerBase
{

    [HttpGet]
    [Route("events")]
    public async Task<ActionResult<EventsMetadataDTO>> GetEventsMetadata()
    {
        var metadata = await metadataService.GetEventsMetadata();
        return Ok(metadata);
    }

    [HttpGet]
    [Route("users")]
    public async Task<ActionResult<UserMetaDataDTO>> GetUsersMetadata()
    {
        var metadata = await metadataService.GetUsersMetadata();
        return Ok(metadata);
    }

}