using MakerSchedule.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MakerSchedule.Domain.Aggregates.User;

namespace MakerSchedule.Application.DomainUsers.Commands;

public class UpdateUserProfileCommandHandler(
    IApplicationDbContext context,
    UserManager<User> userManager,
    ILogger<UpdateUserProfileCommandHandler> logger) : IRequestHandler<UpdateUserProfileCommand, bool>
{
    public async Task<bool> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userDTO = request.UpdateUserProfileDTO;
        var userId = request.UserId;
          var domainUser = await context.DomainUsers.Include(du => du.User).FirstOrDefaultAsync(du => du.UserId == userId);
        if (domainUser == null) return false;

        bool userWasUpdated = false;
        if (userDTO.FirstName != null) { domainUser.FirstName = userDTO.FirstName; userWasUpdated = true; }
        if (userDTO.LastName != null) { domainUser.LastName = userDTO.LastName; userWasUpdated = true; }
        if (userDTO.Address != null) { domainUser.Address = userDTO.Address; userWasUpdated = true; }
        if (userDTO.IsActive.HasValue) { domainUser.IsActive = userDTO.IsActive.Value; userWasUpdated = true; }
        if (userDTO.PhoneNumber != null) { domainUser.PreferredContactMethod = userDTO.PhoneNumber; userWasUpdated = true; }
        if (userDTO.Email != null)
        {
            domainUser.User.Email = userDTO.Email;
            domainUser.User.UserName = userDTO.Email;
            domainUser.User.NormalizedEmail = userManager.KeyNormalizer.NormalizeEmail(userDTO.Email);
            domainUser.User.NormalizedUserName = userManager.KeyNormalizer.NormalizeName(userDTO.Email);
            userWasUpdated = true;
        }

        if (userWasUpdated)
        {
            context.DomainUsers.Update(domainUser);
            context.Users.Update(domainUser.User);
            await context.SaveChangesAsync();
        }
        return userWasUpdated;
    }
}
