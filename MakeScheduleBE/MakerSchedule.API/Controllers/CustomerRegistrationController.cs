using MakerSchedule.Application.DTOs.CustomerRegistration;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers
{
    [ApiController]
    [Route("api/customer")]
    [Produces("application/json")]

    public class CustomerRegistrationController : ControllerBase
    {
        private readonly ICustomerRegistrationService _customerRegService;

        public CustomerRegistrationController(ICustomerRegistrationService customerRegService)
        {
            _customerRegService = customerRegService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CustomerRegistrationDTO dto)
        {
            var result = await _customerRegService.RegisterAsync(dto);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
