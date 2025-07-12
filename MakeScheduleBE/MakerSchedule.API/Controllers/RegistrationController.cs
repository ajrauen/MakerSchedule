using MakerSchedule.Application.DTO.DomainUserRegistration;
using MakerSchedule.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/register")]
[Produces("application/json")]
public class RegistrationController : ControllerBase
{
    private readonly IDomainUserProfileService _domainUserProfileService;

    public RegistrationController(IDomainUserProfileService domainUserProfileService)
    {
        _domainUserProfileService = domainUserProfileService;
    }

    [HttpPost]
    public async Task<IActionResult> Register(CreateDomainUserDTO dto)
    {
        var newUserId = await _domainUserProfileService.CreateDomainUserAsync(dto);
        if (newUserId > 0)
        {
            return Ok(new { id = newUserId });
        }
        return BadRequest("User could not be created.");
    }
}
