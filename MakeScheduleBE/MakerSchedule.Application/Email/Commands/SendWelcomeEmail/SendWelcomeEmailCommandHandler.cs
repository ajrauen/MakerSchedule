using MakerSchedule.Application.Constants;
using MakerSchedule.Application.DTO.EmailRequest;
using MakerSchedule.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.SendEmail.Commands;

public class SendWelcomeEmailCommandHandler(IEmailService emailService, ILogger<SendWelcomeEmailCommandHandler> logger) 
    : IRequestHandler<SendWelcomeEmailCommand, bool>
{
    public async Task<bool> Handle(SendWelcomeEmailCommand request, CancellationToken cancellationToken)
    {

        var template = await emailService.LoadEmailTemplateAsync(EmailTemplateFileName.Welcome);
        var content = emailService.ProcessTemplatePlaceHolders(template, new Dictionary<string, object>
    {
        { "FirstName", request.ToEmail },
        { "Username", request.WelcomeEmailModel.Username }
    });

        var emailRequest = new EmailRequest
        {
            To = request.ToEmail,
            Subject = "Welcome to MakerSchedule",
            Body = content,
            IsHtml = true
        };

        await emailService.SendAsync(emailRequest);
        logger.LogInformation("Welcome email sent to {Email}", request.ToEmail);
        return true;
    }
}
