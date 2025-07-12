using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.Aggregates.DomainUser;
using MakerSchedule.Application.DTO.DomainUserRegistration;

namespace MakerSchedule.Application.Services;

public class DomainUserProfileService(IUserService userService, IDomainUserService employeeService, IApplicationDbContext context, UserManager<User> userManager, ILogger<DomainUserProfileService> logger) : IDomainUserProfileService
{
    private readonly IUserService _userService = userService;
    private readonly IDomainUserService _domainUserService = employeeService;
    private readonly IApplicationDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ILogger<DomainUserProfileService> _logger = logger;

    public async Task<int> CreateDomainUserAsync(CreateDomainUserDTO dto)
    {
        _logger.LogInformation("Attempting to create user with email: {Email}", dto.Email);

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
        };

        var userResult = await _userManager.CreateAsync(user, dto.Password);
        if (!userResult.Succeeded)
        {
            var errors = userResult.Errors.Select(e => e.Description);

            if (errors.Any(e => e.Contains("already taken") || e.Contains("duplicate") || e.Contains("Email")))
                throw new EmailAlreadyExistsException(dto.Email);

            _logger.LogError("Failed to create user: {Errors}", string.Join(", ", errors));
            throw new InvalidOperationException($"Failed to create user: {string.Join(", ", errors)}");
        }

        var domainUser = new DomainUser
        {
            UserId = user.Id,
        };

        _context.DomainUsers.Add(domainUser);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully created user with ID: {DomainUserId}", domainUser.Id);

        return domainUser.Id;
    }

    public async Task<IdentityResult> RegisterDomainUserAsync(CreateDomainUserDTO registrationDto)
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
                var domainUser = new DomainUser
                {
                    UserId = user.Id,
                    PreferredContactMethod = registrationDto.PreferredContactMethod
                };
                _context.DomainUsers.Add(domainUser);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User registered successfully: {Email}", registrationDto.Email);
                // Optionally sign in the user here if needed
            }
            else
            {
                _logger.LogWarning("Failed to register user: {Email}. Errors: {Errors}",
                    registrationDto.Email,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", registrationDto.Email);
            throw;
        }
    }



    public async Task<IdentityResult> RegisterEmployeeAsync(DomainUserRegistrationDTO registrationDto)
    {
        // For compatibility, call RegisterDomainUserAsync with mapped fields
        var createDto = new CreateDomainUserDTO
        {
            Email = registrationDto.Email,
            Password = registrationDto.Password,
            FirstName = registrationDto.FirstName,
            LastName = registrationDto.LastName,
            PhoneNumber = registrationDto.PhoneNumber,
            Address = registrationDto.Address,
            PreferredContactMethod = registrationDto.PreferredContactMethod
        };
        return await RegisterDomainUserAsync(createDto);
    }

    public async Task<bool> UpdateUserProfileAsync(int id, UpdateUserProfileDTO dto)
    {
        var domainUser = await _context.DomainUsers.FindAsync(id);
        if (domainUser == null)
        {
            _logger.LogWarning("No domain user found for ID {Id}", id);
            return false;
        }

        var user = await _userManager.FindByIdAsync(domainUser.UserId);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found.", domainUser.UserId);
            return false;
        }

        bool userWasUpdated = UserProfileUpdater.UpdateUserFields(user, dto, _userManager);

        if (userWasUpdated)
        {
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
        }

        return true;
    }
}