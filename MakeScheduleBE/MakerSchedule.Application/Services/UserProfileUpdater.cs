using MakerSchedule.Application.DTO.User;
using MakerSchedule.Domain.Aggregates.User;

using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Services;

public static class UserProfileUpdater
{
    public static bool UpdateUserFields(User user, UserProfileUpdateFields dto, UserManager<User> userManager)
    {
        bool userWasUpdated = false;
        if (dto.FirstName != null) { user.FirstName = dto.FirstName; userWasUpdated = true; }
        if (dto.LastName != null) { user.LastName = dto.LastName; userWasUpdated = true; }
        if (dto.Email != null)
        {
            user.Email = dto.Email;
            user.UserName = dto.Email;
            user.NormalizedEmail = userManager.KeyNormalizer.NormalizeEmail(dto.Email);
            user.NormalizedUserName = userManager.KeyNormalizer.NormalizeName(dto.Email);
            userWasUpdated = true;
        }
        if (dto.PhoneNumber != null) { user.PhoneNumber = dto.PhoneNumber; userWasUpdated = true; }
        if (dto.Address != null) { user.Address = dto.Address; userWasUpdated = true; }
        if (dto.IsActive.HasValue) { user.IsActive = dto.IsActive.Value; userWasUpdated = true; }
        return userWasUpdated;
    }
} 