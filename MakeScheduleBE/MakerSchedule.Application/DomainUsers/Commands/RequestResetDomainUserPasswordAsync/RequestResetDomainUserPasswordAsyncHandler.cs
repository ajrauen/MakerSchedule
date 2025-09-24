using MakerSchedule.Application.DomainUsers.Commands;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.SendEmail.Commands;
using MakerSchedule.Application.Services.Email.Models;
using MakerSchedule.Domain.Aggregates.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.DomainUsers.Commands;

public class RequestResetDomainUserPasswordAsyncHandler(
    UserManager<User> userManager,
    IMediator mediator,
    ILogger<RequestResetDomainUserPasswordAsyncHandler> logger) : IRequestHandler<RequestResetDomainUserPasswordAsync, bool>
{
    public async Task<bool> Handle(RequestResetDomainUserPasswordAsync request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email.Value);

        if (user == null) 
        {
            logger.LogWarning("Password reset attempted for non-existent user: {Email}", request.Email);
            throw new NotFoundException("User", request.Email.ToString());
        }

        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

        var resetLink = $"{request.BaseUrl}/password-reset?token={Uri.EscapeDataString(resetToken)}&userId={user.Id}";

        var userName = user.UserName ?? user.Email!;
        logger.LogInformation("Creating password reset email for user: {Email}, UserName: {UserName}", user.Email, userName);

        var passwordResetEmailCommand = new SendPasswordResetEmailCommand(user.Email!, new PasswordResetEmailModel
        {
            UserName = userName,
            ResetLink = resetLink
        });

        _ = Task.Run(async () => await mediator.Send(passwordResetEmailCommand, CancellationToken.None));

        logger.LogInformation("Password reset email sent to user: {Email}", user.Email);
        return true;
    }
}