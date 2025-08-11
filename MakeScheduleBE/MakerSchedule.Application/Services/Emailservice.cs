using System.Net.Mail;

using MakerSchedule.Application.DTO.EmailRequest;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.ValueObjects;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class EmailService(IConfiguration configuration, ILogger<Email> logger) : IEmailService
{

    public async Task SendAsync(EmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.To) || string.IsNullOrWhiteSpace(request.Subject) || string.IsNullOrWhiteSpace(request.Body))
        {
            throw new ArgumentException("To, Subject, and Body cannot be empty.");
        }
        try
        {
            var client = createSmtpClient();
            var message = createMessage(request);
            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending email");
            throw;
        }

    }

    public async Task SendWelcomeEmailAsync(string templateName, string toEmail, Dictionary<string, object> data)
    {

        var testDictionay = new Dictionary<string, object>
        {
            { "FirstName", "Christy" },
            { "Username", "Hot Rod" },
        };
        var template = await LoadEmailTemplateAsync("register-welcome.html");
        var content = ProcessTemplatePlaceHolders(template, testDictionay);

        var request = new EmailRequest
        {
            To = "christyfennewald@gmail.com",
            Subject = "Welcome to MakerSchedule",
            Body = content,
            IsHtml = true
        };

        await SendAsync(request);
    }

    private SmtpClient createSmtpClient()
    {
        var host = configuration["Email:SmtpHost"];
        var port = int.Parse(configuration["Email:SmtpPort"]);
        var user = configuration["Email:SmtpUser"];
        var pass = configuration["Email:SmtpPass"];
        var enableSsl = bool.Parse(configuration["Email:EnableSsl"] ?? "true");

        if (string.IsNullOrWhiteSpace(host) || port <= 0 || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
        {
            throw new InvalidOperationException("Email configuration is invalid.");
        }

        return new SmtpClient(host, port)
        {
            Credentials = new System.Net.NetworkCredential(user, pass),
            EnableSsl = enableSsl
        };
    }

    private MailMessage createMessage(EmailRequest request)
    {
        var fromEmail = configuration["Email:SMTPUser"];

        if (string.IsNullOrEmpty(fromEmail))
        {
            throw new InvalidOperationException("From email is not configured.");
        }

        var message = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = request.Subject,
            Body = request.Body,
            IsBodyHtml = request.IsHtml
        };

        message.To.Add(request.To);

        if (!string.IsNullOrWhiteSpace(request.Cc))
        {
            message.CC.Add(request.Cc);
        }

        if (!string.IsNullOrWhiteSpace(request.Bcc))
        {
            message.Bcc.Add(request.Bcc);
        }

        return message;
    }

    private async Task<string> LoadEmailTemplateAsync(string templateName)
    {
                // Try bin directory first (where files are copied)
        var binPath = Path.Combine("bin", "Debug", "net8.0", "EmailTemplates", templateName);
        
        if (File.Exists(binPath))
        {
            logger.LogInformation("Found template at: {Path}", Path.GetFullPath(binPath));
            return await File.ReadAllTextAsync(binPath);
        }
        
        // Fallback to project directory (shouldn't be needed but just in case)
        var projectPath = Path.Combine("EmailTemplates", templateName);
        
        if (File.Exists(projectPath))
        {
            logger.LogInformation("Found template at: {Path}", Path.GetFullPath(projectPath));
            return await File.ReadAllTextAsync(projectPath);
        }
        
        throw new FileNotFoundException($"Email template '{templateName}' not found. Checked: {Path.GetFullPath(binPath)} and {Path.GetFullPath(projectPath)}");


        // var templatePath = Path.Combine("EmailTemplates", templateName);

        // if (!File.Exists(templatePath))
        // {
        //     throw new FileNotFoundException($"Email template '{templateName}' not found at path '{templatePath}'.");
        // }

        // return await File.ReadAllTextAsync(templatePath);
    }

    private string ProcessTemplatePlaceHolders(string template, Dictionary<string, object> data)
    {
        foreach (var kvp in data)
        {
            var placeholder = $"{{{{{kvp.Key}}}}}";
            template = template.Replace(placeholder, kvp.Value?.ToString() ?? string.Empty);
        }
        
        return template;
    }

}