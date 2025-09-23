
using MediatR;

namespace MakerSchedule.Application.DomainUsers.Commands;

public record ResetDomainUserWithTokenAsync(Guid guid, string Token, string NewPassword) : IRequest<bool>;