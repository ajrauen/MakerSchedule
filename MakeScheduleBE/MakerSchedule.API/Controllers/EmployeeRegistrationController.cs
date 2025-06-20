using MakerSchedule.Application.DTOs;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Register(EmployeeRegistrationDTO dto)
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
