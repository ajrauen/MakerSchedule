using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;
        public UserService(UserManager<User> userManager, ILogger<UserService> logger)
        {

            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger;
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var existingUser = await _userManager.FindByIdAsync(user.Id);

            if (existingUser == null)
            {
                var error = new IdentityError { Description = $"User with ID '{user.Id}' not found." };
                return IdentityResult.Failed(error);
            }

            // Map the properties from the incoming user object to the one from the database.
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Address = user.Address;
            existingUser.IsActive = user.IsActive;
            existingUser.UpdatedAt = DateTime.UtcNow;

            // You might also need to update the normalized fields if you change email/username
            existingUser.NormalizedEmail = _userManager.KeyNormalizer.NormalizeEmail(user.Email);
            existingUser.NormalizedUserName = _userManager.KeyNormalizer.NormalizeName(user.Email);
            existingUser.UserName = user.Email;


            var result = await _userManager.UpdateAsync(existingUser);

            if (!result.Succeeded)
            {
                _logger.LogError("Failed to update user {UserId}. Errors: {Errors}", existingUser.Id, string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result;
        }

    }
}