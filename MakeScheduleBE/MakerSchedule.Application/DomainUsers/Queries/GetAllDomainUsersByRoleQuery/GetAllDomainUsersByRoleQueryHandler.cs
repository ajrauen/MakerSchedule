using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MakerSchedule.Domain.Aggregates.User;

namespace MakerSchedule.Application.DomainUsers.Queries;

public class GetAllDomainUsersByRoleQueryHandler(
    IApplicationDbContext context,
    UserManager<User> userManager,
    ILogger<GetAllDomainUsersByRoleQueryHandler> logger) : IRequestHandler<GetAllDomainUsersByRoleQuery, IEnumerable<DomainUserListDTO>>
{
    public async Task<IEnumerable<DomainUserListDTO>> Handle(GetAllDomainUsersByRoleQuery request, CancellationToken cancellationToken)
    {
        var role = request.Role;
        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException("Role cannot be null or empty", nameof(request.Role));
        }

       var usersInRole = await userManager.GetUsersInRoleAsync(role);
        var userIds = usersInRole.Select(u => u.Id).ToList();

        var domainUsers = await context.DomainUsers.Include(du => du.User)
            .Where(du => userIds.Contains(du.UserId))
            .ToListAsync();

        return domainUsers.Select(du => new DomainUserListDTO
        {
            Id = du.Id,
            FirstName = du.FirstName,
            LastName = du.LastName,
            Roles = Array.Empty<string>(),
            Email = du.Email.ToString()
        });
    }
}
