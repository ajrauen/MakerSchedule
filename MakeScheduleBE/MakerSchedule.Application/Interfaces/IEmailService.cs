using MakerSchedule.Application.Constants;
using MakerSchedule.Application.DTO.EmailRequest;
using MakerSchedule.Application.Services.Email.Models;

namespace MakerSchedule.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
    Task<string> LoadEmailTemplateAsync(EmailTemplateFileName template);
    string ProcessTemplatePlaceHolders(string template, Dictionary<string, object> placeholders);
}