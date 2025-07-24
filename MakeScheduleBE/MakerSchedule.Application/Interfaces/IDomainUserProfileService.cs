
using MakerSchedule.Application.DTO.DomainUserRegistration;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Domain.Aggregates.User;

using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Interfaces;

public interface IDomainUserProfileService
{
    Task<Guid> CreateDomainUserAsync(CreateDomainUserDTO dto);
    Task<IdentityResult> RegisterDomainUserAsync(CreateDomainUserDTO registrationDto);
    Task<bool> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDTO dto);
}