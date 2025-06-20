using MakerSchedule.Application.DTOs;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<User> _signInManager;
        public AuthenticationService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<bool> LoginAsync(LoginDTO login) {
            var result = await _signInManager.PasswordSignInAsync(
                    login.Email,
                    login.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);
                    
            return result.Succeeded;
        }

    }
   

}

