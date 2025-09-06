using MakerSchedule.Application.DTO.DomainUserRegistration;
using MediatR;

namespace MakerSchedule.Application.DomainUsers.Commands;

public record CreateDomainUserCommand(CreateDomainUserDTO CreateDomainUserDTO) : IRequest<Guid>;
