using AutoMapper;

using MakerSchedule.Application.DTOs.Employee;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.Employee;
using MakerSchedule.Domain.Aggregates.User;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class EmployeeRegistrationService : IEmployeeRegistrationService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<EmployeeRegistrationService> _logger;
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;
    private const string EmployeePrefix = "EP";

    public EmployeeRegistrationService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<EmployeeRegistrationService> logger,
        IMapper mapper,
        IApplicationDbContext context)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }
    public async Task<IdentityResult> RegisterEmployeeAsync(EmployeeRegistrationDTO registrationDto)
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
                const int maxRetries = 5;
                for (int attempt = 0; attempt < maxRetries; attempt++)
                {
                    var employeeID = await generateEmployeeNumberAsync();
                    var employee = new Employee
                    {
                        UserId = user.Id,
                        EmployeeNumber = employeeID,
                    };
                    _context.Employees.Add(employee);
                    try
                    {
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Employee registered successfully: {Email}", registrationDto.Email);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        break; // Success
                    }
                    catch (DbUpdateException dbEx) when (dbEx.InnerException?.Message.Contains("UNIQUE") == true)
                    {
                        if (attempt == maxRetries - 1)
                        {
                            _logger.LogError(dbEx, "Failed to register employee after multiple attempts due to duplicate EmployeeNumber.");
                            throw;
                        }
                    }
                }
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
    private async Task<string> generateEmployeeNumberAsync()
    {
        int nextEmployeeNumber = 1;
        int maxAttempts = 10_00;
        var lastEmployee = await _context.Employees
          .OrderByDescending(e => e.EmployeeNumber)
          .FirstOrDefaultAsync();

        if (lastEmployee != null && int.TryParse(lastEmployee.EmployeeNumber.Replace(EmployeePrefix, ""), out int lastNumber))
        {
            nextEmployeeNumber = lastNumber + 1;
        }

        for (int i = 0; i < maxAttempts; i++)
        {
            string candidateEmployeeNumber = $"{EmployeePrefix}{nextEmployeeNumber}";
            bool exist = await _context.Employees.AnyAsync(e => e.EmployeeNumber == $"{EmployeePrefix}i");
            if (!exist)
            {
                return candidateEmployeeNumber;
            }
            nextEmployeeNumber++;
        }
        throw new Exception("Can not generate a uqiue employee number");

    }

}

