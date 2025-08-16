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
       return BadRequest($"Failed to send email:");
    }
}