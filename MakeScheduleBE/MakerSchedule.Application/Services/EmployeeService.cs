using System.Collections.Generic;
using System.Threading.Tasks;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.DTOs.Employee;
using AutoMapper;

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

        public async Task<EmployeeIdListDTO> GetAllEmployeeIdsAsync()
        {
            try
            {
                var ids = await _context.Employees.Select(employee => employee.Id).ToListAsync();
                return new EmployeeIdListDTO { Ids = ids };
            } catch (Exception ex) {
                _logger.LogError(ex, "Error fetch employee Ids");
                throw new BaseException("Failed to fetch employee IDs", "FETCH_ERROR", 500, ex);
            }
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    throw new NotFoundException("Employee", id);
                }
                return employee;
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

        public async Task<Employee> CreateEmployeeAsync(CreateEmployeeDTOp createEmployeeDTO)
        {
            try
            {
                var employee = _mapper.Map<Employee>(createEmployeeDTO);
                employee.Id = Guid.NewGuid();
                employee.CreatedAt = DateTime.UtcNow;
                employee.UpdatedAt = null;

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                throw new BaseException("Failed to create employee", "CREATE_ERROR", 500, ex);
            }
        }

        public async Task<Employee> UpdateEmployeeAsync(Guid id, UpdateEmployeeDTO updateEmployeeDTO)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    throw new NotFoundException("Employee", id);
                }

                _mapper.Map(updateEmployeeDTO, employee);
                employee.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return employee;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee: {Id}", id);
                throw new BaseException("Failed to update employee", "UPDATE_ERROR", 500, ex);
            }
        }
        
        public async Task<int> DeleteEmployeeByIdAsync(Guid id)
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