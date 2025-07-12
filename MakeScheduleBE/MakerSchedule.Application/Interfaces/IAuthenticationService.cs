using MakerSchedule.Application.DTO.Authentication;

namespace MakerSchedule.Application.Interfaces;

public interface IAuthenticationService
{
    public Task<(string AccessToken, string RefreshToken)?> LoginAsync(LoginDTO loginDTO);
    public Task<(string AccessToken, string RefreshToken)?> RefreshAsync(string refreshToken);
    public Task<bool> LogoutAsync(string userId);
}
