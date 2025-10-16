
using MakerSchedule.Domain.ValueObjects;
using MediatR;

namespace MakerSchedule.Application.DomainUsers.Commands;

public record ResetDomainUserWithTokenAsync(Email Email, string Token, string NewPassword) : IRequest<bool>;