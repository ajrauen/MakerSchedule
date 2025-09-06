using MediatR;

namespace MakerSchedule.Application.DomainUsers.Commands;

public record DeleteDomainUserByIdCommand(Guid Id) : IRequest<bool>;
