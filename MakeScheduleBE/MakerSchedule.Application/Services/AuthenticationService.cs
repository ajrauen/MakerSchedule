using MakerSchedule.Application.DTO.Authentication;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.User;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MakerSchedule.Application.Services;

public class AuthenticationService(UserManager<User> userManager, JwtService jwtService, IApplicationDbContext context) : IAuthenticationService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly JwtService _jwtService = jwtService;
    private readonly IApplicationDbContext _context = context;

    public async Task<(string AccessToken, string RefreshToken)?> LoginAsync(LoginDTO login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
        {
            return null;
        }

        var accessToken = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = GetNewFreshTokenExpireDate();
        await _userManager.UpdateAsync(user);

        return (accessToken, refreshToken);
    }

    public async Task<(string AccessToken, string RefreshToken)?> RefreshAsync(string refreshToken)
    {
        // Use UserManager to find user by refresh token to avoid tracking conflicts
        var users = await _userManager.Users.Where(u => u.RefreshToken == refreshToken).ToListAsync();
        var user = users.FirstOrDefault();

        if (user == null) {
            // To prevent token reuse, you might want to invalidate the user's session here
            // if you detect a token that was supposed to be invalidated.
            return null;
        }

        // Check if the refresh token is expired.
        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            // If the token is expired, but it's within a short grace period,
            // it might be a race condition from a legitimate client.
            // Let's check if the token expiry is within the last minute.
            if (user.RefreshTokenExpiryTime > DateTime.UtcNow.AddMinutes(-1))
            {
                 // This could be a legitimate race condition.
                 // In this case, we can choose to issue a new token.
                 // For simplicity here, we'll just proceed.
            }
            else
            {
                // The token is truly expired.
                return null;
            }
        }

        var newAccessToken = _jwtService.GenerateToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return (newAccessToken, newRefreshToken);
    }

    public async Task<bool> LogoutAsync(string usider)
    {
        var user = await _userManager.FindByIdAsync(usider);

        if (user == null) {
            return false;
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    private DateTime GetNewFreshTokenExpireDate()
    {
        return DateTime.UtcNow.AddHours(8);
    }
}

