using MakerSchedule.Application.DTO.EmailRequest;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MakerSchedule.Application.Constants;
using MakerSchedule.Application.Interfaces;

namespace MakerSchedule.Application.SendEmail.Commands;

public class SendClassCanceledEmailCommandHandler(IConfiguration configuration, IEmailService emailService, ILogger<SendClassCanceledEmailCommandHandler> logger)
    : IRequestHandler<SendClassCanceledEmailCommand, bool>
{
    public async Task<bool> Handle(SendClassCanceledEmailCommand request, CancellationToken cancellationToken)
    {
         var from = configuration["Email:SMTPUser"];
        var template = await emailService.LoadEmailTemplateAsync(EmailTemplateFileName.ClassCanceled);
        var content = emailService.ProcessTemplatePlaceHolders(template, new Dictionary<string, object>
        {
            { "StudentName", request.Model.StudentName },
            { "EventName", request.Model.EventName },
            { "ScheduleTime", request.Model.ScheduleTime },
            { "Location", request.Model.Location }
        });

        var emailRequest = new EmailRequest
        {
            To = request.ToEmail,
            Subject = "Class Canceled Notification",
            Body = content,
            IsHtml = true
        };

       await emailService.SendAsync(emailRequest);
        logger.LogInformation("Class canceled email sent to {Email}", request.ToEmail);
        return true;
    }
}
