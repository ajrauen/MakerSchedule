using MakerSchedule.Application.DTOs.CustomerRegistration;
using MakerSchedule.Application.DTOs.Employee;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/register")]
[Produces("application/json")]

public class RegistrationController : ControllerBase
{
    private readonly ICustomerRegistrationService _customerRegistrationService;
    private readonly IEmployeeRegistrationService _employeeRegistrationService;

    public RegistrationController(ICustomerRegistrationService customerRegistrationService, IEmployeeRegistrationService employeeRegistrationService)
    {
        _customerRegistrationService = customerRegistrationService;
        _employeeRegistrationService = employeeRegistrationService;
    }

    [HttpPost("customer")]
    public async Task<IActionResult> Register(CustomerRegistrationDTO dto)
    {
        var result = await _customerRegistrationService.RegisterCustomerAsync(dto);
        if (result.Succeeded)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost("employee")]
    public async Task<IActionResult> Register(EmployeeRegistrationDTO dto)
    {
        var result = await _employeeRegistrationService.RegisterEmployeeAsync(dto);
        if (result.Succeeded)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

}
