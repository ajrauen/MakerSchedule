using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using MakerSchedule.Application.Interfaces;
using MakerSchedule.Infrastructure.Data;
using MakerSchedule.Application.Services;
using MakerSchedule.Application.DTOs.Employee;
using MakerSchedule.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Services
{
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
                    bool userWasUpdated = false;
                    if (dto.FirstName != null) { user.FirstName = dto.FirstName; userWasUpdated = true; }
                    if (dto.LastName != null) { user.LastName = dto.LastName; userWasUpdated = true; }
                    if (dto.Email != null)
                    {
                        user.Email = dto.Email;
                        user.UserName = dto.Email;
                        user.NormalizedEmail = _userManager.KeyNormalizer.NormalizeEmail(dto.Email);
                        user.NormalizedUserName = _userManager.KeyNormalizer.NormalizeName(dto.Email);
                        userWasUpdated = true;
                    }

                    if (dto.PhoneNumber != null) { user.PhoneNumber = dto.PhoneNumber; userWasUpdated = true; }
                    if (dto.Address != null) { user.Address = dto.Address; userWasUpdated = true; }
                    if (dto.IsActive.HasValue) { user.IsActive = dto.IsActive.Value; userWasUpdated = true; }

                    if (userWasUpdated)
                    {
                        user.UpdatedAt = DateTime.UtcNow;
                        var userResult = await _userManager.UpdateAsync(user);
                        if (!userResult.Succeeded)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }
                    }

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
}