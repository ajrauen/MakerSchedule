using Microsoft.AspNetCore.Mvc;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.DTO.EmailRequest;
using MakerSchedule.Application.DTO.DomainUserRegistration;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly IDomainUserProfileService _domainUserService;

    public TestController(IEmailService emailService, IDomainUserProfileService domainUserService)
    {
        _emailService = emailService;
        _domainUserService = domainUserService;
    }

    [HttpPost("create-test-user-1")]
    public async Task<IActionResult> CreateTestUser1()
    {
        try
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var dto = new CreateDomainUserDTO
            {
                Email ="andrewrauen@gmail.com",
                Password = "TestPassword123!",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "5552221234",
                Address = "123 Test Street, Test City, TC 12345",
                PreferredContactMethod = "Email"
            };

            var userId = await _domainUserService.CreateDomainUserAsync(dto);
            return Ok(new { UserId = userId, Email = dto.Email, Message = "Test user 1 created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("create-random-test-user")]
    public async Task<IActionResult> CreateRandomTestUser()
    {
        try
        {
            var random = new Random();
            var randomId = random.Next(1000, 9999);
            
            var dto = new CreateDomainUserDTO
            {
                Email = $"testuser{randomId}@test.com",
                Password = "TestPassword123!",
                FirstName = $"TestUser{randomId}",
                LastName = "Generated",
                PhoneNumber = $"555-{randomId:D4}",
                Address = $"{randomId} Random Street, Test City, TC {randomId}",
                PreferredContactMethod = random.Next(2) == 0 ? "Email" : "Phone"
            };

            var userId = await _domainUserService.CreateDomainUserAsync(dto);
            return Ok(new { UserId = userId, Email = dto.Email, Message = "Random test user created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("send-test-email")]
    public IActionResult SendTestEmail()
    {
       return BadRequest($"Failed to send email:");
    }
}