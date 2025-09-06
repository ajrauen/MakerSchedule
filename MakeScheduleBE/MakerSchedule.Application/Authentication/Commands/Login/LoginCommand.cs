using MakerSchedule.Application.DTO.Authentication;
using MediatR;

namespace MakerSchedule.Application.Authentication.Commands;

public record LoginCommand(LoginDTO LoginDTO) : IRequest<(string AccessToken, string RefreshToken)?>;
