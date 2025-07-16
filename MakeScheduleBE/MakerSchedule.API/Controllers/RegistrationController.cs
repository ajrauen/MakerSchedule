using MakerSchedule.Application.DTO.DomainUserRegistration;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/register")]
[Produces("application/json")]
public class RegistrationController(IDomainUserProfileService domainUserProfileService, IUserService userService) : ControllerBase
{


    [HttpPost]
    public async Task<IActionResult> Register(CreateDomainUserDTO dto)
    {
        var newUserId = await domainUserProfileService.CreateDomainUserAsync(dto);
        if (!string.IsNullOrEmpty(newUserId))
        {
            return Ok(new { id = newUserId });
        }
        return BadRequest("User could not be created.");
    }
    
    // [HttpGet()]
    // [Route("api/Events/leaders")]
    // public async Task<ActionResult<IEnumerable<LeaderDTO>> GetAvailableLeaders(int id)
    // {
    //     var eventItem = await userService.GetAvailableOccurrenceLeaders();
    //     return Ok(eventItem);
    // }

}
