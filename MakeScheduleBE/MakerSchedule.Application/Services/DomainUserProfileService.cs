using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.Aggregates.DomainUser;
using MakerSchedule.Domain.ValueObjects;
using MakerSchedule.Application.DTO.DomainUserRegistration;
using MakerSchedule.Application.Services.Email.Models;

namespace MakerSchedule.Application.Services;

public class DomainUserProfileService(  IApplicationDbContext context, UserManager<User> userManager, ILogger<DomainUserProfileService> logger, IEmailService emailService) : IDomainUserProfileService
{


    public async Task<Guid> CreateDomainUserAsync(CreateDomainUserDTO dto)
    {
        logger.LogInformation("Attempting to create user with email: {Email}", dto.Email);

        // Create the User
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var userResult = await userManager.CreateAsync(user, dto.Password);
        if (!userResult.Succeeded)
        {
            var errors = userResult.Errors.Select(e => e.Description);

            if (errors.Any(e => e.Contains("already taken") || e.Contains("duplicate") || e.Contains("Email")))
                throw new EmailAlreadyExistsException(dto.Email);

            logger.LogError("Failed to create user: {Errors}", string.Join(", ", errors));
            throw new InvalidOperationException($"Failed to create user: {string.Join(", ", errors)}");
        }

        var domainUser = new DomainUser
        {
            UserId = user.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            PreferredContactMethod = dto.PreferredContactMethod,
            Email = new MakerSchedule.Domain.ValueObjects.Email(dto.Email),
            PhoneNumber = new PhoneNumber(dto.PhoneNumber)
        };

        context.DomainUsers.Add(domainUser);
        await context.SaveChangesAsync();

        await emailService.SendWelcomeEmailAsync(domainUser.Email.Value, new WelcomeEmailModel
        {
            FirstName = domainUser.FirstName,
        });

        logger.LogInformation("Successfully created user with ID: {DomainUserId}", domainUser.Id);

        return domainUser.Id;
    }

    public async Task<IdentityResult> RegisterDomainUserAsync(CreateDomainUserDTO registrationDto)
    {
        try
        {
            var user = new User
            {
                UserName = registrationDto.Email,
                Email = registrationDto.Email
            };

            var result = await userManager.CreateAsync(user, registrationDto.Password);

            if (result.Succeeded)
            {
                var domainUser = new DomainUser
                {
                    UserId = user.Id,
                    FirstName = registrationDto.FirstName,
                    LastName = registrationDto.LastName,
                    Address = registrationDto.Address,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    PreferredContactMethod = registrationDto.PreferredContactMethod,
                    Email = new MakerSchedule.Domain.ValueObjects.Email(registrationDto.Email),
                    PhoneNumber = new PhoneNumber(registrationDto.PhoneNumber)
                };
                context.DomainUsers.Add(domainUser);
                await context.SaveChangesAsync();
                logger.LogInformation("User registered successfully: {Email}", registrationDto.Email);
            }
            else
            {
                logger.LogWarning("Failed to register user: {Email}. Errors: {Errors}",
                    registrationDto.Email,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

           await emailService.SendWelcomeEmailAsync(registrationDto.Email, new WelcomeEmailModel
            {
                FirstName = registrationDto.FirstName,
            });

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error registering user: {Email}", registrationDto.Email);
            throw;
        }
    }

    public async Task<bool> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDTO dto)
    {
        var domainUser = await context.DomainUsers.Include(du => du.User).FirstOrDefaultAsync(du => du.UserId == userId);
        if (domainUser == null) return false;

        bool userWasUpdated = false;
        if (dto.FirstName != null) { domainUser.FirstName = dto.FirstName; userWasUpdated = true; }
        if (dto.LastName != null) { domainUser.LastName = dto.LastName; userWasUpdated = true; }
        if (dto.Address != null) { domainUser.Address = dto.Address; userWasUpdated = true; }
        if (dto.IsActive.HasValue) { domainUser.IsActive = dto.IsActive.Value; userWasUpdated = true; }
        if (dto.PhoneNumber != null) { domainUser.PreferredContactMethod = dto.PhoneNumber; userWasUpdated = true; }
        if (dto.Email != null)
        {
            domainUser.User.Email = dto.Email;
            domainUser.User.UserName = dto.Email;
            domainUser.User.NormalizedEmail = userManager.KeyNormalizer.NormalizeEmail(dto.Email);
            domainUser.User.NormalizedUserName = userManager.KeyNormalizer.NormalizeName(dto.Email);
            userWasUpdated = true;
        }

        if (userWasUpdated)
        {
            context.DomainUsers.Update(domainUser);
            context.Users.Update(domainUser.User);
            await context.SaveChangesAsync();
        }
        return userWasUpdated;
    }
}