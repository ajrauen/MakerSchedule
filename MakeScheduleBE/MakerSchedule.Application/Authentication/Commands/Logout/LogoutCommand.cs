using MediatR;

namespace MakerSchedule.Application.Authentication.Commands;

public record LogoutCommand(string UserId) : IRequest<bool>;
