using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MakerSchedule.API.Services;

/// <summary>
/// Implementation of user authorization service for checking resource access.
/// </summary>
public class UserAuthorizationService : IUserAuthorizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAuthorizationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthorizedForUserResource(Guid resourceUserId)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (user == null)
        {
            return false;
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var currentUserId))
        {
            return false;
        }

        var isAdmin = user.IsInRole("Admin");
        var isOwner = resourceUserId == currentUserId;

        return (isAdmin || isOwner);
    }

    public Guid? GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (user == null)
        {
            return null;
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return null;
        }

        return userId;
    }
}
