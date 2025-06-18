using MakerSchedule.Application.DTOs.EmployeeRegistration;
using MakerSchedule.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.API.Controllers
{
    [ApiController]
    [Route("api/employee")]
    [Produces("application/json")]

    public class EmployeeRegistrationController : ControllerBase
    {
        private readonly IEmployeeRegistrationService _employeeRegService;

        public EmployeeRegistrationController(IEmployeeRegistrationService employeeRegService)
        {
            _employeeRegService = employeeRegService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(EmployeeRegristrationDTO dto)
        {
            var result = await _employeeRegService.RegisterAsync(dto);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}