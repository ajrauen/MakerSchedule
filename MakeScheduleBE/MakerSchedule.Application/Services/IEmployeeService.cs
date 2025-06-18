using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Application.DTOs.Employee;

namespace MakerSchedule.Application.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync();
        Task<EmployeeIdListDTO> GetAllEmployeeIdsAsync();
        Task<Employee> GetEmployeeByIdAsync(string id);
        Task<Employee> CreateEmployeeAsync(CreateEmployeeDTOp createEmployeeDTO);
        Task<Employee> UpdateEmployeeAsync(string id, UpdateEmployeeDTO updateEmployeeDTO);
        Task<int> DeleteEmployeeByIdAsync(string id);
    }
}