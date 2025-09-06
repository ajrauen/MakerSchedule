using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.Services;
using MakerSchedule.Domain.Aggregates.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Authentication.Commands;

public class LoginCommandHandler(
    UserManager<User> userManager,
    JwtService jwtService,
    ILogger<LoginCommandHandler> logger) : IRequestHandler<LoginCommand, (string AccessToken, string RefreshToken)?>
{
    public async Task<(string AccessToken, string RefreshToken)?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var login = request.LoginDTO;
        logger.LogInformation("Attempting login for email: {Email}", login.Email);

        var user = await userManager.FindByEmailAsync(login.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, login.Password))
        {
            logger.LogWarning("Login failed for email: {Email}", login.Email);
            return null;
        }

        var accessToken = jwtService.GenerateToken(user);
        var refreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = GetNewRefreshTokenExpireDate();
        await userManager.UpdateAsync(user);

        logger.LogInformation("Successfully logged in user: {Email}", login.Email);
        return (accessToken, refreshToken);
    }

    private DateTime GetNewRefreshTokenExpireDate()
    {
        return DateTime.UtcNow.AddHours(8);
    }
}
