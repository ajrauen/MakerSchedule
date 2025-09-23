using MakerSchedule.Domain.ValueObjects;

using MediatR;

namespace MakerSchedule.Application.DomainUsers.Commands;

public record RequestResetDomainUserPasswordAsync(Email Email, string BaseUrl) : IRequest<bool>;