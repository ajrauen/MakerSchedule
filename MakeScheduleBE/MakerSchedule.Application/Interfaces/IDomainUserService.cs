using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Domain.Aggregates.DomainUser;
using MakerSchedule.Domain.Constants;

using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Interfaces;

public interface IDomainUserService
{
    Task<IEnumerable<DomainUser>> GetAllDomainUsersWithDetailsAsync();
    Task<IEnumerable<DomainUserListDTO>> GetAllDomainUsersAsync();
    Task<DomainUserDTO> GetDomainUserByIdAsync(string id);
    Task DeleteDomainUserByIdAsync(string id);

    Task<IEnumerable<LeaderDTO>> GetAvailableOccurrenceLeadersAsync(string occurrenceId);
    Task<IEnumerable<DomainUserListDTO>> GetAllDomainUsersByRoleAsync(string role);

}
