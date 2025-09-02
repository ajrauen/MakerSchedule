using MakerSchedule.Application.Services.Email.Models;

using MediatR;

namespace MakerSchedule.Application.SendEmail.Commands;

public record SendWelcomeEmailCommand(
    string ToEmail,
    WelcomeEmailModel WelcomeEmailModel
) : IRequest<bool>;
