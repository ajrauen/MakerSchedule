using MakerSchedule.Application.DTOs.CustomerRegistration;
using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Interfaces;

public interface ICustomerRegistrationService
{
    Task<IdentityResult> RegisterCustomerAsync(CustomerRegistrationDTO registrationDto);
} 