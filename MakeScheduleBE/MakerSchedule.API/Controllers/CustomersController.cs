using System.ComponentModel.DataAnnotations;

using MakerSchedule.Application.DTOs.Customer;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Application.Services;
using MakerSchedule.Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ICustomerProfileService _customerProfileService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerService customerService, ICustomerProfileService customerProfileService, ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _customerProfileService = customerProfileService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerListDTO>>> GetAllCustomersAsync()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        return Ok(customer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerByIdAsync(int id)
    {
        await _customerService.DeleteCustomerByIdAsync(id);
        return NoContent();

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerProfile(int id, [FromBody] UpdateCustomerProfileDTO dto)
    {
        var success = await _customerProfileService.UpdateCustomerProfileAsync(id, dto);
        if (success)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomerProfile(CreateCustomerDTO customerDTO)
    {
        var success = await _customerProfileService.CreateCustomerAsync(customerDTO);
        if (success > 0)
        {
            return Ok(success);
        }

        return BadRequest("Could not create Customer");
        
    }

}
