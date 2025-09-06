using MakerSchedule.Application.DTO.DomainUser;
using MediatR;

namespace MakerSchedule.Application.DomainUsers.Queries;

public record GetDomainUserByIdQuery(Guid Id) : IRequest<DomainUserDTO>;
