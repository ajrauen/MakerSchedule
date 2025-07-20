using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.DTO.DomainUserRegistration;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/domain-users")]
[Produces("application/json")]
public class DomainUsersController(IDomainUserService domainUserService, IDomainUserProfileService domainUserProfileService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DomainUserListDTO>>> GetAllDomainUsersAsync([FromQuery] string? role = null)
    {
        var users = string.IsNullOrEmpty(role) 
            ? await domainUserService.GetAllDomainUsersAsync()
            : await domainUserService.GetAllDomainUsersByRoleAsync(role);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DomainUserDTO>> GetById(Guid id)
    {
        var user = await domainUserService.GetDomainUserByIdAsync(id);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDomainUserByIdAsync(Guid id)
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
        if (newUserId != Guid.Empty)
        {
            return CreatedAtAction(nameof(GetById), new { id = newUserId }, new { id = newUserId });
        }
        return BadRequest("User could not be created.");
    }

     [HttpPost]
    // [Authorize(Roles = "Admin")]
    [Route("available-leaders")]
    public async Task<ActionResult<IEnumerable<DomainUserListDTO>>> GetAvailableLeaders([FromBody] GetAvailableLeadersRequest request)
    {
        IEnumerable<DomainUserListDTO> leaders = await domainUserService.GetAvailableOccurrenceLeadersAsync(request.StartTime, request.Duration, request.CurrentLeaderIds ?? []);

        return Ok(leaders);
    }
}
