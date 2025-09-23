using MakerSchedule.Application.DomainUsers.Queries;
using MakerSchedule.Application.DomainUsers.Commands;
using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.DTO.DomainUserRegistration;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Application.Interfaces;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MakerSchedule.Domain.ValueObjects;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/domain-users")]
[Produces("application/json")]
public class DomainUsersController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DomainUserListDTO>>> GetAllDomainUsersAsync([FromQuery] string? role = null)
    {
        if (string.IsNullOrEmpty(role))
        {
            var command = new GetAllDomainUsersQuery();
            var users = await mediator.Send(command);
            return Ok(users);
        }
        else
        {
            var query = new GetAllDomainUsersByRoleQuery(role);
            var users = await mediator.Send(query);
            return Ok(users);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DomainUserDTO>> GetById(Guid id)
    {
        var query = new GetDomainUserByIdQuery(id);
        var user = await mediator.Send(query);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDomainUserByIdAsync(Guid id)
    {
        var command = new DeleteDomainUserByIdCommand(id);
        var success = await mediator.Send(command);

        if (success)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDomainUserProfile(Guid id, [FromBody] UpdateUserProfileDTO dto)
    {
        var command = new UpdateUserProfileCommand(id, dto);
        var success = await mediator.Send(command);

        if (success)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDomainUserProfile([FromBody] CreateDomainUserDTO domainUserDTO)
    {
        var command = new CreateDomainUserCommand(domainUserDTO);
        var newUserId = await mediator.Send(command);

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
        var query = new GetAvailableLeadersQuery(request.StartTime, request.Duration, request.CurrentLeaderIds ?? [], request.OccurrenceId);
        var leaders = await mediator.Send(query);
        return Ok(leaders);
    }

    [HttpPost]
    [Route("request-reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var email = new Email(request.Email);
        var command = new RequestResetDomainUserPasswordAsync(email, $"{Request.Scheme}://{Request.Host}");
        var result = await mediator.Send(command);

        return Ok(new { Message = "If an account with that email exists, a password reset link has been sent." });
    }

    [HttpPost]
    [Route("validate-reset-password-token")]
    public async Task<IActionResult> ValidateResetToken([FromBody] ValidateResetTokenRequest request)
    {
        var command = new ValidateResetDomainUserTokenAsync(request.UserId, request.Token);
        var isValid = await mediator.Send(command);

        if (isValid)
        {
            return Ok(new { Message = "The reset token is valid." });
        }

        return BadRequest(new { Message = "The reset token is invalid or has expired." });
    }

    [HttpPost]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordWithTokenRequest request)
    {
        var command = new ResetDomainUserWithTokenAsync(request.UserId, request.Token, request.NewPassword);
        var isSuccess = await mediator.Send(command);
     
        return Ok(new { Message = "The password has been reset successfully." });

    }

}
