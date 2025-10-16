using MediatR;
namespace MakerSchedule.Application.DomainUsers.Commands;

public record ChangeDomainUserPassword(Guid id, string currentPassword, string newPassword) : IRequest<bool>;