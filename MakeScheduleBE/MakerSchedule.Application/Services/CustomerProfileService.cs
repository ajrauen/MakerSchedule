using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using MakerSchedule.Application.Interfaces;
using MakerSchedule.Infrastructure.Data;
using MakerSchedule.Application.Services;
using MakerSchedule.Application.DTOs.Customer;
using MakerSchedule.Application.DTOs.User;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Application.Services;

public class CustomerProfileService : ICustomerProfileService
{
    private readonly IUserService _userService;
    private readonly ICustomerService _customerService; // Assume this only updatesCustomer-specific fields now
    private readonly ApplicationDbContext _context; // It needs the DbContext to manage the transaction
    private readonly UserManager<User> _userManager;
    private readonly ILogger<CustomerProfileService> _logger;

    public CustomerProfileService(IUserService userService, ICustomerService customerService, ApplicationDbContext context, UserManager<User> userManager, ILogger<CustomerProfileService> logger)
    {
        _userService = userService;
        _customerService = customerService;
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<int> CreateCustomerAsync(CreateCustomerDTO dto)
    {
        _logger.LogInformation("Attempting to create customer with email: {Email}", dto.Email);

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
                    UserType = UserType.Customer
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

                // Create the Customer
                var customer = new Customer
                {
                    UserId = user.Id,
                    CustomerNumber = GenerateCustomerNumber(),
                    PreferredContactMethod = dto.PreferredContactMethod ?? string.Empty,
                    Notes = dto.Notes ?? string.Empty
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation("Successfully created customer with ID: {CustomerId}", customer.Id);

                // Return the customer ID
                return customer.Id;
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
                _logger.LogError(ex, "Unexpected error creating customer");
                throw new InvalidOperationException("An unexpected error occurred while creating the customer", ex);
            }
        }
    }

    private string GenerateCustomerNumber()
    {
        // Generate a unique customer number (you can implement your own logic)
        return $"CUST-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }

    public async Task<bool> UpdateCustomerProfileAsync(int id, UpdateCustomerProfileDTO dto)
    {
        _logger.LogInformation("Attempting to update profile for UserId: {UserId}. Received FirstName: {FirstName}", id, dto.FirstName);

        // Step 1: Find the User by their primary key from the URL.
  
        // Step 2: Find the associatedCustomer record using the UserId.
        var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Id == id);
        if (customer == null)
        {
            _logger.LogWarning("No customer found for User with ID {UserId}", id);
            return false;
        }

        var user = await _userManager.FindByIdAsync(customer.UserId);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found.", customer.UserId);
            return false;
        }

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                bool userWasUpdated = UserProfileUpdater.UpdateUserFields(user, dto, _userManager);

                bool customerWasUpdated = false;
                if (dto.Notes != null) { customer.Notes = dto.Notes; customerWasUpdated = true; }

                if (customerWasUpdated)
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