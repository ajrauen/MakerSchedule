using MakerSchedule.Application.DTO;
using MakerSchedule.Application.DTO.Authentication; // <-- This is the missing line
using MakerSchedule.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private const string REFRESH_TOKEN_COOKIE_NAME = "refreshToken";
    
    private readonly IAuthenticationService _authService;
    private readonly IWebHostEnvironment _env;

    public AuthController(IAuthenticationService authService, IWebHostEnvironment env)
    {
        _authService = authService;
        _env = env;
    }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO login)
        {
            // Add debugging
            Console.WriteLine($"Login attempt for email: {login.Email}");

            var result = await _authService.LoginAsync(login);

            Console.WriteLine($"Login result: {result.HasValue}");

            if (result.HasValue)
            {
                Console.WriteLine($"Setting refresh token and returning access token");
                SetRefreshTokenInCookie(result.Value.RefreshToken);
                var response = new LoginResponseDTO { AccessToken = result.Value.AccessToken };
                Console.WriteLine($"Response object: {System.Text.Json.JsonSerializer.Serialize(response)}");
                return Ok(response);
            }

            Console.WriteLine($"Login failed - returning Unauthorized");
            return Unauthorized();
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshAsync()
        {
            var refreshToken = Request.Cookies[REFRESH_TOKEN_COOKIE_NAME];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Missing refresh token.");
            }

            var result = await _authService.RefreshAsync(refreshToken);

            if (result.HasValue)
            {
                SetRefreshTokenInCookie(result.Value.RefreshToken);
                return Ok(new LoginResponseDTO { AccessToken = result.Value.AccessToken });
            }

            return Unauthorized("Invalid refresh token.");
        }

    [HttpPost]
    [Route("logout")]
    [Authorize]
    public async Task<IActionResult> LogOutAsync()
    {
        Console.WriteLine($"[DEBUG] Server time (UTC) is: {DateTime.UtcNow:o}");
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("Invalid request");
        }

        var success = await _authService.LogoutAsync(userId);

        if (success) {
            Response.Cookies.Delete(REFRESH_TOKEN_COOKIE_NAME);
            return Ok();
        }
        return BadRequest("Invalid request");
    }

    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        };

        Response.Cookies.Append(REFRESH_TOKEN_COOKIE_NAME, refreshToken, cookieOptions);
    }
}