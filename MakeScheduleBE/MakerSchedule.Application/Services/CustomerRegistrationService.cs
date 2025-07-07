using AutoMapper;

using MakerSchedule.Application.DTOs.CustomerRegistration;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Infrastructure.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class CustomerRegistrationService : ICustomerRegistrationService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<CustomerRegistrationService> _logger;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public CustomerRegistrationService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<CustomerRegistrationService> logger,
        IMapper mapper,
        ApplicationDbContext context)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }

    public async Task<IdentityResult> RegisterCustomerAsync(CustomerRegistrationDTO registrationDto)
    {
        try
        {
            var user = new User
            {
                UserName = registrationDto.Email,
                Email = registrationDto.Email,
                PhoneNumber = registrationDto.PhoneNumber,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                Address = registrationDto.Address,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, registrationDto.Password);

            if (result.Succeeded)
            {
                var customer = new Customer
                {
                    UserId = user.Id,
                    CustomerNumber = registrationDto.CustomerNumber,
                    PreferredContactMethod = registrationDto.PreferredContactMethod,
                    Notes = registrationDto.Notes
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Customer registered successfully: {Email}", registrationDto.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            else
            {
                _logger.LogWarning("Failed to register customer: {Email}. Errors: {Errors}",
                    registrationDto.Email,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering customer: {Email}", registrationDto.Email);
            throw;
        }
    }
}

