using MakerSchedule.Domain.ValueObjects;
using MediatR;

namespace MakerSchedule.Application.DomainUsers.Commands;

public record ValidateResetDomainUserTokenAsync(Email Email, string Token) : IRequest<bool>;