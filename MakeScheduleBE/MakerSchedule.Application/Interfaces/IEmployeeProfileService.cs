using MakerSchedule.Application.DTOs.Employee;
using System.Threading.Tasks;

namespace MakerSchedule.Application.Interfaces
{
    public interface IEmployeeProfileService
    {
        Task<bool> UpdateEmployeeProfileAsync(int userId, UpdateEmployeeProfileDTO dto);
    }
}