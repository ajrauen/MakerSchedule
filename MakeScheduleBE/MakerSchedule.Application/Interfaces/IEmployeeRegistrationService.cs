using MakerSchedule.Application.DTOs.Employee;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MakerSchedule.Application.Interfaces
{
    public interface IEmployeeRegistrationService
    {
        Task<IdentityResult> RegisterEmployeeAsync(EmployeeRegistrationDTO registrationDTO);
    }
} 