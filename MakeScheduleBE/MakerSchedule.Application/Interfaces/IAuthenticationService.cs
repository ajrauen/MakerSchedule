using MakerSchedule.Application.DTOs;

namespace MakerSchedule.Application.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<bool> LoginAsync(LoginDTO loginDTO);
    }


}
