using MakerSchedule.Application.DTO.EmailRequest;

namespace MakerSchedule.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
    Task SendWelcomeEmailAsync(string templateName, string toEmail, Dictionary<string, object> data);
}