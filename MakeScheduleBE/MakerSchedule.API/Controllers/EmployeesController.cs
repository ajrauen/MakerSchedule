using Microsoft.AspNetCore.Mvc;
using MakerSchedule.Application.Services;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Application.DTOs.Employee;
using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployeesAsync()
        {
            var employees = await _employeeService.GetAllEmployeesWithDetailsAsync();
            return Ok(employees);
        }


        [HttpGet]
        [Route("idsonly")]
        public async Task<ActionResult<EmployeeIdListDTO>> GetEmployeeIdsAsync()
        {
            var employeeIds = await _employeeService.GetAllEmployeeIdsAsync();
            return Ok(employeeIds);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(Guid id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync(CreateEmployeeDTOp createEmployeeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    error = "Validation failed",
                    code = "VALIDATION_ERROR",
                    details = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            var employee = await _employeeService.CreateEmployeeAsync(createEmployeeDTO);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeByIdAsync(Guid id, [FromBody] UpdateEmployeeDTO updateEmployeeDTO)
        {
            var employee = await _employeeService.UpdateEmployeeAsync(id, updateEmployeeDTO);
            return Ok(employee);
        }

    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeByIdAsync(Guid id)
        {
            await _employeeService.DeleteEmployeeByIdAsync(id);
             return NoContent();

        }
    }
} 