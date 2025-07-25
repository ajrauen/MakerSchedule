using MakerSchedule.Application.DTO.Metadata;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.Services;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MetadataController(IEventsMetadataService eventsMetadataService) : ControllerBase
{

    [HttpGet]
    [Route("events")]
    public async Task<ActionResult<EventsMetadataDTO>> GetEventsMetadata()
    {
        var metadata = await eventsMetadataService.GetEventsMetadata();
        return Ok(metadata);
    }

}