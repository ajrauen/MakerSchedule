using MakerSchedule.Application.DomainUsers.Commands;
using MakerSchedule.Domain.Aggregates.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.DomainUsers.Commands;

public class ValidateResetDomainUserTokenAsyncHandler(
    UserManager<User> userManager,
    ILogger<ValidateResetDomainUserTokenAsyncHandler> logger) : IRequestHandler<ValidateResetDomainUserTokenAsync, bool>
{
    public async Task<bool> Handle(ValidateResetDomainUserTokenAsync request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Validating password reset token for user: {UserId}", request.guid);

        var user = await userManager.FindByIdAsync(request.guid.ToString());

        if (user == null) 
        {
            logger.LogWarning("Password reset token validation failed - user not found: {UserId}", request.guid);
            return false; // Don't reveal that user doesn't exist
        }

        var isValidToken = await userManager.VerifyUserTokenAsync(
            user, 
            userManager.Options.Tokens.PasswordResetTokenProvider, 
            "ResetPassword", 
            request.Token);

        if (!isValidToken)
        {
            logger.LogWarning("Password reset token validation failed for user: {UserId}", request.guid);
            return false;
        }

        logger.LogInformation("Password reset token validated successfully for user: {Email}", user.Email);
        return true;
    }
}