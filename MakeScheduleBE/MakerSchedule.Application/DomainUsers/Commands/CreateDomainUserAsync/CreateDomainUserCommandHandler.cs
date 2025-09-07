using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.SendEmail.Commands;
using MakerSchedule.Application.Services.Email.Models;
using MakerSchedule.Domain.Aggregates.DomainUser;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.DomainUsers.Commands;

public class CreateDomainUserCommandHandler(
    IApplicationDbContext context,
    UserManager<User> userManager,
    IMediator mediator,
    ILogger<CreateDomainUserCommandHandler> logger) : IRequestHandler<CreateDomainUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateDomainUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CreateDomainUserDTO;
        logger.LogInformation("Attempting to create user with email: {Email}", dto.Email);

        // Use transaction to ensure both user creations succeed or both rollback
        using var transaction = await ((DbContext)context).Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // Create the Identity User
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

            // Create the Domain User
            var domainUser = new DomainUser
            {
                UserId = user.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                PreferredContactMethod = dto.PreferredContactMethod,
                Email = new Email(dto.Email),
                PhoneNumber = new PhoneNumber(dto.PhoneNumber)
            };

            context.DomainUsers.Add(domainUser);
            await context.SaveChangesAsync(cancellationToken);

            // Commit the transaction
            await transaction.CommitAsync(cancellationToken);

            // Send welcome email (fire and forget - don't block the response)
            var welcomeEmailCommand = new SendWelcomeEmailCommand(domainUser.Email.Value, new WelcomeEmailModel
            {
                FirstName = domainUser.FirstName,
            });

            _ = Task.Run(async () => await mediator.Send(welcomeEmailCommand, CancellationToken.None));

            logger.LogInformation("Successfully created user with ID: {DomainUserId}", domainUser.Id);
            return domainUser.Id;
        }
        catch
        {
            // Rollback on any exception
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
