using MakerSchedule.Application.DomainUsers.Commands;
using MakerSchedule.Application.DTO.DomainUserRegistration;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Application.Interfaces;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/register")]
[Produces("application/json")]
public class RegistrationController(IMediator mediator ) : ControllerBase
{


    [HttpPost]
    public async Task<IActionResult> Register(CreateDomainUserDTO dto)
    {
        var command  = new CreateDomainUserCommand(dto);
        var newUserId = await mediator.Send(command);
        if (newUserId != Guid.Empty)
        {
            return Ok(new { id = newUserId });
        }
        return BadRequest("User could not be created.");
    }

 

}
