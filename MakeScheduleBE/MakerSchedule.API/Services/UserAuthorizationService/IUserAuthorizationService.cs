namespace MakerSchedule.API.Services;

public interface IUserAuthorizationService
{
    bool IsAuthorizedForUserResource(Guid resourceUserId);
    Guid? GetCurrentUserId();
}
