using MakerSchedule.Application.Services;
using MakerSchedule.Domain.Aggregates.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Authentication.Commands;

public class RefreshTokenCommandHandler(
    UserManager<User> userManager,
    JwtService jwtService,
    ILogger<RefreshTokenCommandHandler> logger) : IRequestHandler<RefreshTokenCommand, (string AccessToken, string RefreshToken)?>
{
    public async Task<(string AccessToken, string RefreshToken)?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to refresh token");

        // Use UserManager to find user by refresh token to avoid tracking conflicts
        var users = await userManager.Users.Where(u => u.RefreshToken == request.RefreshToken).ToListAsync(cancellationToken);
        var user = users.FirstOrDefault();

        if (user == null) 
        {
            logger.LogWarning("Refresh token not found or invalid");
            return null;
        }

        // Check if the refresh token is expired
        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            // If the token is expired, but it's within a short grace period,
            // it might be a race condition from a legitimate client
            if (user.RefreshTokenExpiryTime > DateTime.UtcNow.AddMinutes(-1))
            {
                logger.LogInformation("Token expired but within grace period for user: {UserId}", user.Id);
                // This could be a legitimate race condition, proceed
            }
            else
            {
                logger.LogWarning("Refresh token expired for user: {UserId}", user.Id);
                return null;
            }
        }

        var newAccessToken = jwtService.GenerateToken(user);
        var newRefreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        logger.LogInformation("Successfully refreshed token for user: {UserId}", user.Id);
        return (newAccessToken, newRefreshToken);
    }
}
