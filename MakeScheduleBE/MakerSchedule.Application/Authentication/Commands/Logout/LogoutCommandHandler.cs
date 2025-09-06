using MakerSchedule.Domain.Aggregates.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Authentication.Commands;

public class LogoutCommandHandler(
    UserManager<User> userManager,
    ILogger<LogoutCommandHandler> logger) : IRequestHandler<LogoutCommand, bool>
{
    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting logout for user: {UserId}", request.UserId);

        var user = await userManager.FindByIdAsync(request.UserId);

        if (user == null) 
        {
            logger.LogWarning("User not found for logout: {UserId}", request.UserId);
            return false;
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        var result = await userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            logger.LogInformation("Successfully logged out user: {UserId}", request.UserId);
        }
        else
        {
            logger.LogError("Failed to logout user {UserId}: {Errors}", 
                request.UserId, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return result.Succeeded;
    }
}
