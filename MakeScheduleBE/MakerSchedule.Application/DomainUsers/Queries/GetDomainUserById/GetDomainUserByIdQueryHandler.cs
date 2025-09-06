using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.DomainUsers.Queries;

public class GetDomainUserByIdQueryHandler(
    IApplicationDbContext context,
    ILogger<GetDomainUserByIdQueryHandler> logger) : IRequestHandler<GetDomainUserByIdQuery, DomainUserDTO>
{
    public async Task<DomainUserDTO> Handle(GetDomainUserByIdQuery request, CancellationToken cancellationToken)
    {
        var domainUser = await context.DomainUsers
            .Where(du => du.Id == request.Id)
            .Select(du => new DomainUserDTO
            {
                Id = du.Id,
                UserId = du.UserId,
                Email = du.Email,
                FirstName = du.FirstName,
                LastName = du.LastName,
                PhoneNumber = du.PhoneNumber,
                Address = du.Address,
                IsActive = du.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (domainUser == null)
        {
            logger.LogWarning("Domain user not found: {UserId}", request.Id);
            throw new KeyNotFoundException($"Domain user with ID {request.Id} not found.");
        }

        return domainUser;
    }
}
