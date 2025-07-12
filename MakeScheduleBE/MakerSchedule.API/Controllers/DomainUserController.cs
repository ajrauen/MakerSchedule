using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.DTO.DomainUserRegistration;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DomainUsersController : ControllerBase
{
    private readonly IDomainUserService _domainUserService;
    private readonly IDomainUserProfileService _domainUserProfileService;
    private readonly ILogger<DomainUsersController> _logger;

    public DomainUsersController(IDomainUserService domainUserService, IDomainUserProfileService domainUserProfileService, ILogger<DomainUsersController> logger)
    {
        _domainUserService = domainUserService;
        _domainUserProfileService = domainUserProfileService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DomainUserListDTO>>> GetAllDomainUsersAsync()
    {
        var users = await _domainUserService.GetAllDomainUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DomainUserDTO>> GetById(int id)
    {
        var user = await _domainUserService.GetDomainUserByIdAsync(id);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDomainUserByIdAsync(int id)
    {
        await _domainUserService.DeleteDomainUserByIdAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDomainUserProfile(int id, [FromBody] UpdateUserProfileDTO dto)
    {
        var success = await _domainUserProfileService.UpdateUserProfileAsync(id, dto);
        if (success)
        {
            return NoContent();
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDomainUserProfile([FromBody] CreateDomainUserDTO domainUserDTO)
    {
        var newUserId = await _domainUserProfileService.CreateDomainUserAsync(domainUserDTO);
        if (newUserId > 0)
        {
            return CreatedAtAction(nameof(GetById), new { id = newUserId }, new { id = newUserId });
        }
        return BadRequest("User could not be created.");
    }
}
