using MakerSchedule.Application.DTO.DomainUser;
using MediatR;

namespace MakerSchedule.Application.DomainUsers.Queries;

public record GetAllDomainUsersByRoleQuery(string Role) : IRequest<IEnumerable<DomainUserListDTO>>;
