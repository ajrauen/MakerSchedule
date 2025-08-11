using Microsoft.AspNetCore.Mvc;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.DTO.EmailRequest;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IEmailService _emailService;

    public TestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send-test-email")]
    public async Task<IActionResult> SendTestEmail()
    {
        try
        {
            var request = new EmailRequest
            {
                To = "andrewrauen@gmail.com", // Put YOUR email here
                Subject = "Test Email from MakerSchedule",
                Body = "This is a test email. If you're reading this, the email service works!",
                IsHtml = false
            };

            await _emailService.SendWelcomeEmailAsync("register-welcome", request.To, new Dictionary<string, object>
            {
                { "Subject", request.Subject },
                { "Body", request.Body }
            });
            return Ok("Email sent successfully!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to send email: {ex.Message}");
        }
    }
}