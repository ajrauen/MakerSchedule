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
        logger.LogInformation("Resetting password with token for user: {UserId}", request.guid);

        var user = await userManager.FindByIdAsync(request.guid.ToString());

        if (user == null) 
        {
            logger.LogWarning("Password reset failed - user not found: {UserId}", request.guid);
            return false; // Don't reveal that user doesn't exist
        }

        // Reset the password using the token and new password
        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            logger.LogWarning("Password reset failed for user {UserId}: {Errors}", 
                request.guid, string.Join(", ", errors));
            return false;
        }

        logger.LogInformation("Password successfully reset for user: {Email}", user.Email);
        return true;
    }
}