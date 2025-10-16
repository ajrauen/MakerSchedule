using MakerSchedule.Application.DomainUsers.Commands;
using MakerSchedule.Domain.Aggregates.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.DomainUsers.Commands;

public class ResetDomainUserWithTokenAsyncHandler(
    UserManager<User> userManager,
    ILogger<ResetDomainUserWithTokenAsyncHandler> logger) : IRequestHandler<ResetDomainUserWithTokenAsync, bool>
{
    public async Task<bool> Handle(ResetDomainUserWithTokenAsync request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Resetting password with token for user: {Email}", request.Email);

        var user = await userManager.FindByEmailAsync(request.Email.Value);

        if (user == null) 
        {
            logger.LogWarning("Password reset failed - user not found: {Email}", request.Email);
            return false; 
        }

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            logger.LogWarning("Password reset failed for user {Email}: {Errors}", 
                request.Email, string.Join(", ", errors));
            return false;
        }

        logger.LogInformation("Password successfully reset for user: {Email}", user.Email);
        return true;
    }
}