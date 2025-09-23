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
        logger.LogInformation("Processing password reset email for {Email} with UserName: '{UserName}'", 
            request.ToEmail, request.PasswordResetEmailModel.UserName);

        var template = await emailService.LoadEmailTemplateAsync(EmailTemplateFileName.PasswordReset);
        
        if (string.IsNullOrEmpty(template))
        {
            logger.LogError("Failed to load password reset email template");
            throw new InvalidOperationException("Password reset email template not found");
        }
        
        logger.LogInformation("Template loaded, length: {Length}", template.Length);
        
        var placeholders = new Dictionary<string, object>
        {
            { "UserName", request.PasswordResetEmailModel.UserName },
            { "ResetLink", request.PasswordResetEmailModel.ResetLink }
        };
        
        logger.LogInformation("Template placeholders: UserName='{UserName}', ResetLink='{ResetLink}'", 
            request.PasswordResetEmailModel.UserName, request.PasswordResetEmailModel.ResetLink);
        
        var content = emailService.ProcessTemplatePlaceHolders(template, placeholders);
        
        logger.LogInformation("Processed content contains UserName placeholder: {ContainsUserName}", 
            content.Contains("{{UserName}}"));

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
