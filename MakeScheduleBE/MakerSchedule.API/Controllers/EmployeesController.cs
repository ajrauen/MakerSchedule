using System.ComponentModel.DataAnnotations;

using MakerSchedule.Application.DTOs.Employee;
using MakerSchedule.Application.Services;
using MakerSchedule.Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize]
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
        public async Task<IActionResult> DeleteEmployeeByIdAsync(string id)
        {
            await _employeeService.DeleteEmployeeByIdAsync(id);
            return NoContent();

        }
    }
}
