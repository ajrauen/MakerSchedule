using Microsoft.AspNetCore.Identity;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Application.DTOs.EmployeeRegistration;

namespace MakerSchedule.Application.Interfaces;

public interface IEmployeeRegistrationService
{
    // Registration and Login
    Task<IdentityResult> RegisterAsync(EmployeeRegristrationDTO registrationDto);
}
