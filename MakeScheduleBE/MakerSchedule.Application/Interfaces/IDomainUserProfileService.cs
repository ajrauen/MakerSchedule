
using MakerSchedule.Application.DTO.DomainUserRegistration;
using MakerSchedule.Application.DTO.User;
using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Interfaces;

public interface IDomainUserProfileService
{
    Task<int> CreateDomainUserAsync(CreateDomainUserDTO dto);
    Task<IdentityResult> RegisterDomainUserAsync(CreateDomainUserDTO registrationDto);
    Task<IdentityResult> RegisterEmployeeAsync(DomainUserRegistrationDTO registrationDto);
    Task<bool> UpdateUserProfileAsync(int id, UpdateUserProfileDTO dto);
}