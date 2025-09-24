using MakerSchedule.Application.Constants;
using MakerSchedule.Application.DTO.EmailRequest;
using MakerSchedule.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.SendEmail.Commands;

public class SendPasswordResetEmailCommandHandler(IEmailService emailService, ILogger<SendPasswordResetEmailCommandHandler> logger) 
    : IRequestHandler<SendPasswordResetEmailCommand, bool>
{
    public async Task<bool> Handle(SendPasswordResetEmailCommand request, CancellationToken cancellationToken)
    {
        var template = await emailService.LoadEmailTemplateAsync(EmailTemplateFileName.PasswordReset);
        
        if (string.IsNullOrEmpty(template))
        {
            logger.LogError("Failed to load password reset email template");
            throw new InvalidOperationException("Password reset email template not found");
        }
        
        
        var placeholders = new Dictionary<string, object>
        {
            { "UserName", request.PasswordResetEmailModel.UserName },
            { "ResetLink", request.PasswordResetEmailModel.ResetLink }
        };
        
        var content = emailService.ProcessTemplatePlaceHolders(template, placeholders);


        var emailRequest = new EmailRequest
        {
            To = request.ToEmail,
            Subject = "Password Reset Request",
            Body = content,
            IsHtml = true
        };

        await emailService.SendAsync(emailRequest);
        logger.LogInformation("Password reset email sent to {Email}", request.ToEmail);
        return true;
    }
}
