using MakerSchedule.Application.DTOs.Employee;

using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Interfaces;

public interface IEmployeeRegistrationService
{
    Task<IdentityResult> RegisterEmployeeAsync(EmployeeRegistrationDTO registrationDto);
} 