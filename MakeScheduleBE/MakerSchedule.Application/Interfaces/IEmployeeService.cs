using MakerSchedule.Application.DTOs.Employee;
using MakerSchedule.Domain.Entities;

namespace MakerSchedule.Application.Services;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync();
    Task<IEnumerable<EmployeeListDTO>> GetAllEmployeesAsync();
    Task<EmployeeDTO> GetEmployeeByIdAsync(int id);
    Task DeleteEmployeeByIdAsync(int itd);
}
