using MakerSchedule.Application.DTOs.CustomerRegistration;
using MakerSchedule.Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Interfaces;

public interface ICustomerRegistrationService
{
    // Registration and Login
    Task<IdentityResult> RegisterAsync(CustomerRegistrationDTO registrationDto);
}
