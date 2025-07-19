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
    public async Task<ActionResult<IEnumerable<DomainUserListDTO>>> GetAllDomainUsersAsync()
    {
        var users = await domainUserService.GetAllDomainUsersAsync();
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

    // [HttpGet()]
    // // [Authorize(Roles = "Admin")]
    // [Route("/leaders")]
    // public async Task<ActionResult<IEnumerable<DomainUserListDTO>>> GetAvailableLeaders([FromQuery] string? occurrenceId = null, [FromQuery] string? role = null)
    // {
    //     IEnumerable<DomainUserListDTO> leaders;



    //     if (string.IsNullOrEmpty(occurrenceId))
    //     {
    //         leaders = await domainUserService.GetAvailableOccurrenceLeadersAsync(occurrenceId);
    //     }
    //     else
    //     {
    //         leaders = await domainUserService.GetAllDomainUsersByRoleAsync(role);
    //     }

    //     return Ok(leaders);
    // }

     [HttpGet()]
    // [Authorize(Roles = "Admin")]
    [Route("available-leaders")]
    public async Task<ActionResult<IEnumerable<DomainUserListDTO>>> GetAvailableLeaders([FromQuery] long startTime, [FromQuery] long duration)
    {
        IEnumerable<DomainUserListDTO> leaders;

            leaders = await domainUserService.GetAvailableOccurrenceLeadersAsync(startTime, duration);
      

        return Ok(leaders);
    }
}
