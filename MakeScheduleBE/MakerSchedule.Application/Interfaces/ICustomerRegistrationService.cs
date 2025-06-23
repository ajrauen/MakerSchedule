using MakerSchedule.Application.DTOs.CustomerRegistration;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MakerSchedule.Application.Interfaces
{
    public interface ICustomerRegistrationService
    {
        Task<IdentityResult> RegisterCustomerAsync(CustomerRegistrationDTO registrationDto);
    }
} 