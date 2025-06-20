using MakerSchedule.Application.DTOs;
using MakerSchedule.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace MakerSchedule.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDTO login) {
            var result = await _authService.LoginAsync(login);

            if (result) {
                return Ok();
            };
            return Unauthorized();
        }


    }
}