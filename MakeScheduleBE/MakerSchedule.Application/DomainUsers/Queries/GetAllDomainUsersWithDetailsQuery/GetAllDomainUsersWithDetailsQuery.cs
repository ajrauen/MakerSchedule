using MakerSchedule.Domain.Aggregates.DomainUser;
using MediatR;

namespace MakerSchedule.Application.DomainUsers.Queries;

public record GetAllDomainUsersWithDetailsQuery() : IRequest<IEnumerable<DomainUser>>;
