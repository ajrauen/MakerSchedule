using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.DomainUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.DomainUsers.Queries;

public class GetAllDomainUsersWithDetailsQueryHandler(
    IApplicationDbContext context,
    ILogger<GetAllDomainUsersWithDetailsQueryHandler> logger) : IRequestHandler<GetAllDomainUsersWithDetailsQuery, IEnumerable<DomainUser>>
{
    public async Task<IEnumerable<DomainUser>> Handle(GetAllDomainUsersWithDetailsQuery request, CancellationToken cancellationToken)
    {

        var domainUsers = await context.DomainUsers
            .Include(du => du.User) 
            .Where(du => du.IsActive)
            .ToListAsync(cancellationToken);

            logger.LogInformation("Found {Count} domain users with details", domainUsers.Count);

        return domainUsers;
    }
}
