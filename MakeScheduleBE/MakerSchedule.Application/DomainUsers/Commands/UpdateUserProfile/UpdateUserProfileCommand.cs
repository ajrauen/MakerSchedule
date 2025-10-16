using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.DTO.User;
using MediatR;

namespace MakerSchedule.Application.DomainUsers.Commands;

public record UpdateUserProfileCommand(Guid id, UpdateUserProfileDTO UpdateUserProfileDTO) : IRequest<DomainUserDTO>;
