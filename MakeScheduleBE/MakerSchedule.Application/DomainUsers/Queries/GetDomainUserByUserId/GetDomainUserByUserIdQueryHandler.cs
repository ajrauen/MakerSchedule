using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.User;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.DomainUsers.Queries;

public class GetDomainUserByUserIdQueryHandler(
    IApplicationDbContext context,
     UserManager<User> userManager,
    ILogger<GetDomainUserByUserIdQueryHandler> logger) : IRequestHandler<GetDomainUserByUserIdQuery, DomainUserDTO>
{
    public async Task<DomainUserDTO> Handle(GetDomainUserByUserIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if(user == null)
        {
            logger.LogWarning("User not found: {UserId}", request.UserId);
            throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
        }

        var role = await userManager.GetRolesAsync(user);

        var domainUser = await context.DomainUsers
            .Where(du => du.UserId == request.UserId)
            .Select(du => new DomainUserDTO
            {
                Id = du.Id,
                UserId = du.UserId,
                Email = du.Email.Value,
                FirstName = du.FirstName,
                LastName = du.LastName,
                PhoneNumber = du.PhoneNumber.Value,
                Address = du.Address,
                IsActive = du.IsActive,
                Roles = role.ToArray()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (domainUser == null)
        {
            logger.LogWarning("Domain user not found: {UserId}", request.UserId);
            throw new KeyNotFoundException($"Domain user with ID {request.UserId} not found.");
        }

        return domainUser;
    }
}
