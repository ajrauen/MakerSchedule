using MakerSchedule.Application.DTO.Metadata;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.Services;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MetadataController(IMetadataService metadataService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<MetadataDTO>> GetMetadata()
    {
        var metadata = await metadataService.GetApplicationMetadata();
        return Ok(metadata); 
    }

}