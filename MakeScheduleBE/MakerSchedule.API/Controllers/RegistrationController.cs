using MakerSchedule.Application.DTO.DomainUserRegistration;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/register")]
[Produces("application/json")]
public class RegistrationController(IDomainUserProfileService domainUserProfileService ) : ControllerBase
{


    [HttpPost]
    public async Task<IActionResult> Register(CreateDomainUserDTO dto)
    {
        var newUserId = await domainUserProfileService.CreateDomainUserAsync(dto);
        if (newUserId != Guid.Empty)
        {
            return Ok(new { id = newUserId });
        }
        return BadRequest("User could not be created.");
    }

 

}
