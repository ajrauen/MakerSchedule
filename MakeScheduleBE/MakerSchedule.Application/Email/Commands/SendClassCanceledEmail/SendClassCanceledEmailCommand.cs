using MakerSchedule.Application.Services.Email.Models;

using MediatR;

namespace MakerSchedule.Application.SendEmail.Commands;

public record SendClassCanceledEmailCommand(
    string ToEmail,
    ClassCanceledEmailModel Model
) : IRequest<bool>;
