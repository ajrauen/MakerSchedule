using MakerSchedule.Domain.ValueObjects;

using MediatR;

namespace MakerSchedule.Application.DomainUsers.Commands;

public record ValidateResetDomainUserTokenAsync(Guid guid, string Token) : IRequest<bool>;