using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OccurrencesController : ControllerBase
{
    private readonly IOccurrenceService _occuranceService;

    public OccurrencesController(IOccurrenceService occurrenceService)
    {
        _occuranceService = occurrenceService;
    }

    [HttpGet]
    public async Task<ActionResult<OccurenceListDTO>> GetAllOccurrencesAsync()
    {
        var occurences = await _occuranceService.GetAllOccurrencesAsync();
        return Ok(occurences);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OccurenceDTO>> GetOccurrenceByIdAsync(string id)
    {
        var occurences = await _occuranceService.GetOccurrenceByIdAsync(id);
        return Ok(occurences);
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateOccuranceAsync([FromBody] CreateOccurenceDTO createOccurenceDTO)
    {
        var occurrence = await _occuranceService.CreateOccurrenceAsync(createOccurenceDTO);
        return Ok(occurrence);
    }


}