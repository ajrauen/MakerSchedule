using MakerSchedule.Application.DTOs.Employee;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.Services;
using MakerSchedule.Domain.Aggregates.Employee;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IEmployeeProfileService _employeeProfileService;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IEmployeeService employeeService, IEmployeeProfileService employeeProfileService, ILogger<EmployeesController> logger)
    {
        _employeeService = employeeService;
        _employeeProfileService = employeeProfileService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeListDTO>>> GetAllEmployeesAsync()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        return Ok(employee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeByIdAsync(int id)
    {
        await _employeeService.DeleteEmployeeByIdAsync(id);
        return NoContent();

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeProfile(int id, [FromBody] UpdateEmployeeProfileDTO dto)
    {
        var success = await _employeeProfileService.UpdateEmployeeProfileAsync(id, dto);
        if (success)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployeeProfile([FromBody] CreateEmployeeDTO employeeDTO)
    {
        
        var newEmployeeId = await _employeeProfileService.CreateEmployeeAsync(employeeDTO);
        if (newEmployeeId > 0)
        {
            return CreatedAtAction(nameof(GetById), new { id = newEmployeeId }, new { id = newEmployeeId });
        }
        return BadRequest("Employee could not be created.");
      
    }
}
