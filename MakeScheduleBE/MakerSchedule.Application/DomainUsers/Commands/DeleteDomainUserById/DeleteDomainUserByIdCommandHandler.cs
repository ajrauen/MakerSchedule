using MakerSchedule.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MakerSchedule.Domain.Aggregates.User;

namespace MakerSchedule.Application.DomainUsers.Commands;

public class DeleteDomainUserByIdCommandHandler(
    IApplicationDbContext context,
    UserManager<User> userManager,
    ILogger<DeleteDomainUserByIdCommandHandler> logger) : IRequestHandler<DeleteDomainUserByIdCommand, bool>
{
    public async Task<bool> Handle(DeleteDomainUserByIdCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting domain user with ID: {UserId}", request.Id);

        var domainUser = await context.DomainUsers
            .Include(du => du.User)
            .FirstOrDefaultAsync(du => du.Id == request.Id, cancellationToken);

        if (domainUser == null)
        {
            logger.LogWarning("Domain user not found: {UserId}", request.Id);
            return false;
        }

        using var transaction = await ((DbContext)context).Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            context.DomainUsers.Remove(domainUser);
            await context.SaveChangesAsync(cancellationToken);

            if (domainUser.User != null)
            {
                var result = await userManager.DeleteAsync(domainUser.User);
                if (!result.Succeeded)
                {
                    logger.LogError("Failed to delete Identity user {UserId}: {Errors}", 
                        domainUser.UserId, string.Join(", ", result.Errors.Select(e => e.Description)));
                    
                    await transaction.RollbackAsync(cancellationToken);
                    return false;
                }
            }

            await transaction.CommitAsync(cancellationToken);
            logger.LogInformation("Successfully deleted domain user: {UserId}", request.Id);
            return true;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
