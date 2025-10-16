using MakerSchedule.Application.DTO.DomainUser;
using MediatR;

namespace MakerSchedule.Application.DomainUsers.Queries;

public record GetDomainUserByUserIdQuery(Guid UserId) : IRequest<DomainUserDTO>;
