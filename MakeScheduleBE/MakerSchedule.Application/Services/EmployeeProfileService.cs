using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using MakerSchedule.Application.Interfaces;
using MakerSchedule.Infrastructure.Data;
using MakerSchedule.Application.Services;
using MakerSchedule.Application.DTOs.Employee;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MakerSchedule.Application.DTOs.User;
using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Application.Services;

public class EmployeeProfileService : IEmployeeProfileService
{
    private readonly IUserService _userService;
    private readonly IEmployeeService _employeeService; // Assume this only updates Employee-specific fields now
    private readonly ApplicationDbContext _context; // It needs the DbContext to manage the transaction
    private readonly UserManager<User> _userManager;
    private readonly ILogger<EmployeeProfileService> _logger;

    public EmployeeProfileService(IUserService userService, IEmployeeService employeeService, ApplicationDbContext context, UserManager<User> userManager, ILogger<EmployeeProfileService> logger)
    {
        _userService = userService;
        _employeeService = employeeService;
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<int> CreateEmployeeAsync(CreateEmployeeDTO dto)
    {
        _logger.LogInformation("Attempting to create employee with email: {Email}", dto.Email);

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                // Create the User
                var user = new User
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    UserType = UserType.Employee
                };

                var userResult = await _userManager.CreateAsync(user, dto.Password);
                if (!userResult.Succeeded)
                {
                    var errors = userResult.Errors.Select(e => e.Description);
                    
                    // Check for email already taken error
                    if (errors.Any(e => e.Contains("already taken") || e.Contains("duplicate") || e.Contains("Email")))
                    {
                        throw new EmailAlreadyExistsException(dto.Email);
                    }
                    
                    // Handle other validation errors
                    _logger.LogError("Failed to create user: {Errors}", string.Join(", ", errors));
                    throw new InvalidOperationException($"Failed to create user: {string.Join(", ", errors)}");
                }

                // Create the Employee
                var employee = new Employee
                {
                    UserId = user.Id,
                    EmployeeNumber = dto.EmployeeNumber,
                    Department = dto.Department,
                    Position = dto.Position,
                    HireDate = dto.HireDate
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation("Successfully created employee with ID: {EmployeeId}", employee.Id);

                // Return the employee ID
                return employee.Id;
            }
            catch (BaseException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (InvalidOperationException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Unexpected error creating employee");
                throw new InvalidOperationException("An unexpected error occurred while creating the employee", ex);
            }
        }
    }

    private string GenerateEmployeeNumber()
    {
        // Generate a unique employee number (you can implement your own logic)
        return $"EMP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }

    public async Task<bool> UpdateEmployeeProfileAsync(int id, UpdateEmployeeProfileDTO dto)
    {
        _logger.LogInformation("Attempting to update profile for UserId: {UserId}. Received FirstName: {FirstName}", id, dto.FirstName);

        // Step 1: Find the User by their primary key from the URL.
  
        // Step 2: Find the associated Employee record using the UserId.
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            _logger.LogWarning("No employee found for User with ID {UserId}", id);
            return false;
        }

        var user = await _userManager.FindByIdAsync(employee.UserId);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found.", employee.UserId);
            return false;
        }



        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                bool userWasUpdated = UserProfileUpdater.UpdateUserFields(user, dto, _userManager);

                bool employeeWasUpdated = false;
                if (dto.Department != null) { employee.Department = dto.Department; employeeWasUpdated = true; }
                if (dto.Position != null) { employee.Position = dto.Position; employeeWasUpdated = true; }
                if (dto.HireDate.HasValue) { employee.HireDate = dto.HireDate.Value; employeeWasUpdated = true; }

                if (employeeWasUpdated)
                {
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}