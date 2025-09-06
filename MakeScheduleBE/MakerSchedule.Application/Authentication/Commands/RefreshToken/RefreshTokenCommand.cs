using MediatR;

namespace MakerSchedule.Application.Authentication.Commands;

public record RefreshTokenCommand(string RefreshToken) : IRequest<(string AccessToken, string RefreshToken)?>;
