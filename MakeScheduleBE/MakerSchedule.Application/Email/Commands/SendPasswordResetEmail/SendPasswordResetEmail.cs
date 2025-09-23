using MediatR;
namespace MakerSchedule.Application.SendEmail.Commands;

public record SendPasswordResetEmailCommand(
    string ToEmail,
    PasswordResetEmailModel PasswordResetEmailModel
) : IRequest<bool>;