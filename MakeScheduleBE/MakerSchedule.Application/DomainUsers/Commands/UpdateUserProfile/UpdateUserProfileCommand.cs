using MakerSchedule.Application.DTO.User;
using MediatR;

namespace MakerSchedule.Application.DomainUsers.Commands;

public record UpdateUserProfileCommand(Guid UserId, UpdateUserProfileDTO UpdateUserProfileDTO) : IRequest<bool>;
