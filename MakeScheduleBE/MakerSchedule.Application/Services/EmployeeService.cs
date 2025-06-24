using AutoMapper;

using MakerSchedule.Application.DTOs.Employee;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Infrastructure.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public EmployeeService(
            ApplicationDbContext context,
            ILogger<EmployeeService> logger,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
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
                    FirstName = employee.User?.FirstName ?? string.Empty,
                    LastName = employee.User?.LastName ?? string.Empty,

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
                    .Include(e =>e.EventsLed)
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
                    Email = employee.User?.Email ?? string.Empty,
                    FirstName = employee.User?.FirstName ?? string.Empty,
                    LastName = employee.User?.LastName ?? string.Empty,
                    PhoneNumber = employee.User?.PhoneNumber ?? string.Empty,
                    Address = employee.User?.Address ?? string.Empty,
                    IsActive = employee.User?.IsActive ?? false,
                    EventsLed = employee.EventsLed.Select(e => new EventSummaryDTO
                    {
                        Id = e.Id,
                        EventName = e.EventName,
                    }).ToList(),
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

        public async Task DeleteEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                throw new NotFoundException("Employee", id); 
            }

            var user = await _userManager.FindByIdAsync(employee.UserId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", employee.UserId);
                throw new NotFoundException("User not found", employee.UserId); 
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Employees.Remove(employee);
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        throw new BaseException(
                                    message: $"Failed to delete user '{user.Id}'.",
                                    errorCode: "USER_DELETION_FAILED",
                                    statusCode: 500 // Or another appropriate status code like 400 Bad Request
                                );
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
