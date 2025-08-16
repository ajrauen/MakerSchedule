using MakerSchedule.Application.DTO.EmailRequest;
using MakerSchedule.Application.Services.Email.Models;

namespace MakerSchedule.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
    Task SendWelcomeEmailAsync( string toEmail, WelcomeEmailModel model);

    void SendClassCanceledEmail(ClassCanceledEmailModel model);
}