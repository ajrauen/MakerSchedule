using AutoMapper;

using MakerSchedule.Application.DTOs.Employee;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;

        public EmployeeService(
            ApplicationDbContext context,
            ILogger<EmployeeService> logger,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync()
        {
            try
            {
                return await _context.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employees");
                throw new BaseException("Failed to fetch employees", "FETCH_ERROR", 500, ex);
            }
        }


        public async Task<IEnumerable<EmployeeListDTO>> GetAllEmployeesAsync()
        {
            try
            {
                var employees = await _context.Employees
                                .Include(e => e.User)
                                .ToListAsync();

                var employeeDTOs = employees.Select(employee => new EmployeeListDTO
                {
                    Id = employee.Id,
                    EmployeeID = employee.EmployeeNumber,
                    FirstName = employee.User.FirstName,
                    LastName = employee.User.LastName,

                }).ToList();


                return employeeDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetch employee Ids");
                throw new BaseException("Failed to fetch employee IDs", "FETCH_ERROR", 500, ex);
            }
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var employee = await _context.Employees
                    .Include(e => e.User)
                    .FirstOrDefaultAsync(e => e.Id == id);
                if (employee == null)
                {
                    throw new NotFoundException("Employee", id);
                }
                return new EmployeeDTO
                {
                    Id = employee.Id,
                    EmployeeNumber = employee.EmployeeNumber,
                    Department = employee.Department,
                    Position = employee.Position,
                    HireDate = employee.HireDate,
                    UserId = employee.UserId,
                    Email = employee.User.Email,
                    FirstName = employee.User.FirstName,
                    LastName = employee.User.LastName,
                    PhoneNumber = employee.User.PhoneNumber,
                    Address = employee.User.Address,
                    IsActive = employee.User.IsActive
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee by id: {Id}", id);
                throw new BaseException("Failed to fetch employee", "FETCH_ERROR", 500, ex);
            }
        }

        public async Task<int> DeleteEmployeeByIdAsync(string id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    throw new NotFoundException("Employee", id);
                }
                _context.Employees.Remove(employee);
                return await _context.SaveChangesAsync();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee: {Id}", id);
                throw new BaseException("Failed to delete employee", "DELETE_ERROR", 500, ex);
            }
        }
    }
}
