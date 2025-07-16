using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.DTO.DomainUserRegistration;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/domain-users")]
[Produces("application/json")] 
public class DomainUsersController(IDomainUserService domainUserService, IDomainUserProfileService domainUserProfileService, ILogger<DomainUsersController> logger) : ControllerBase
    {

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DomainUserListDTO>>> GetAllDomainUsersAsync()
    {
        var users = await domainUserService.GetAllDomainUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DomainUserDTO>> GetById(string id)
    {
        var user = await domainUserService.GetDomainUserByIdAsync(id);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDomainUserByIdAsync(string id)
    {
        await domainUserService.DeleteDomainUserByIdAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDomainUserProfile(string id, [FromBody] UpdateUserProfileDTO dto)
    {
        var success = await domainUserProfileService.UpdateUserProfileAsync(id, dto);
        if (success)
        {
            return NoContent();
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDomainUserProfile([FromBody] CreateDomainUserDTO domainUserDTO)
    {
        var newUserId = await domainUserProfileService.CreateDomainUserAsync(domainUserDTO);
        if (!string.IsNullOrEmpty(newUserId))
        {
            return CreatedAtAction(nameof(GetById), new { id = newUserId }, new { id = newUserId });
        }
        return BadRequest("User could not be created.");
    }
}
