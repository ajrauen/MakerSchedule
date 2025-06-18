using AutoMapper;
using MakerSchedule.Application.DTOs.EmployeeRegistration;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class EmployeeRegistrationService : IEmployeeRegistrationService
{
    private readonly UserManager<Employee> _userManager;
    private readonly SignInManager<Employee> _signInManager;
    private readonly ILogger<EmployeeRegistrationService> _logger;
    private readonly IMapper _mapper;

    public EmployeeRegistrationService(
        UserManager<Employee> userManager,
        SignInManager<Employee> signInManager,
        ILogger<EmployeeRegistrationService> logger,
        IMapper mapper)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IdentityResult> RegisterAsync(EmployeeRegristrationDTO registrationDto)
    {
        try
        {
            var employee = new Employee
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

            var result = await _userManager.CreateAsync(employee, registrationDto.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("Employee registered successfully: {Email}", registrationDto.Email);
                await _signInManager.SignInAsync(employee, isPersistent: false);
            }
            else
            {
                _logger.LogWarning("Failed to register employee: {Email}. Errors: {Errors}", 
                    registrationDto.Email, 
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering employee: {Email}", registrationDto.Email);
            throw;
        }
    }
}

