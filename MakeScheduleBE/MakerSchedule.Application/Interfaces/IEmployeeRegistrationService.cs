using MakerSchedule.Application.DTOs.Employee;

using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Interfaces;

public interface IEmployeeRegistrationService
{
    // Registration and Login
    Task<IdentityResult> RegisterAsync(EmployeeRegistrationDTO registrationDTO);
}
