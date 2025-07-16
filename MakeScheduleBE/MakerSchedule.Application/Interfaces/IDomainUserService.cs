using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Domain.Aggregates.DomainUser;

namespace MakerSchedule.Application.Interfaces;

public interface IDomainUserService
{
    Task<IEnumerable<DomainUser>> GetAllDomainUsersWithDetailsAsync();
    Task<IEnumerable<DomainUserListDTO>> GetAllDomainUsersAsync();
    Task<DomainUserDTO> GetDomainUserByIdAsync(string id);
    Task DeleteDomainUserByIdAsync(string id);
}
