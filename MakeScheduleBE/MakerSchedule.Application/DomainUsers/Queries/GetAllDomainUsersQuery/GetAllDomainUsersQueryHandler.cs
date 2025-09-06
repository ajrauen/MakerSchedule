using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

using MediatR;

public class GetAllDomainUsersQueryHandler(IApplicationDbContext context) : IRequestHandler<GetAllDomainUsersQuery, IEnumerable<DomainUserListDTO>>
{

    public async Task<IEnumerable<DomainUserListDTO>> Handle(GetAllDomainUsersQuery request, CancellationToken cancellationToken)
    {
          var userRoles = await (
                from du in context.DomainUsers
                join u in context.Users on du.UserId equals u.Id
                join ur in context.UserRoles on u.Id equals ur.UserId into urj
                from ur in urj.DefaultIfEmpty()
                join r in context.Roles on ur.RoleId equals r.Id into rj
                from r in rj.DefaultIfEmpty()
                select new {
                    DomainUser = du,
                    RoleName = r != null ? r.Name : null
                }
            ).ToListAsync();

            var x = await context.Roles.ToListAsync();


            var grouped = userRoles
                .GroupBy(x => x.DomainUser.Id)
                .Select(g => new DomainUserListDTO
                {
                    Id = g.Key,
                    FirstName = g.First().DomainUser.FirstName,
                    LastName = g.First().DomainUser.LastName,
                    Roles = g.Where(x => x.RoleName != null).Select(x => x.RoleName).ToArray(),
                    Email = g.First().DomainUser.Email.ToString()
                })
                .ToList();

            return grouped;
    }
}