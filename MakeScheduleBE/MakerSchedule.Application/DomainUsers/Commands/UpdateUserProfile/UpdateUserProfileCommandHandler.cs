using MakerSchedule.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.Exceptions;

namespace MakerSchedule.Application.DomainUsers.Commands;

public class UpdateUserProfileCommandHandler(
    IApplicationDbContext context,
    UserManager<User> userManager) : IRequestHandler<UpdateUserProfileCommand, DomainUserDTO>
{
    public async Task<DomainUserDTO> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userDTO = request.UpdateUserProfileDTO;
        var id = request.id;
        var domainUser = await context.DomainUsers.Include(du => du.User).FirstOrDefaultAsync(du => du.Id  == id);
        if (domainUser == null) throw new NullReferenceException($"Domain user with UserId {id} not found.");

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
            return new DomainUserDTO
            {
                Id = domainUser.Id,
                UserId = domainUser.UserId,
                FirstName = domainUser.FirstName,
                LastName = domainUser.LastName,
                Address = domainUser.Address,
                IsActive = domainUser.IsActive,
                PhoneNumber = domainUser.PreferredContactMethod,
                Email = domainUser.User.Email ?? "",
                Roles = (await userManager.GetRolesAsync(domainUser.User)).ToArray()
            };
        }
                 throw new NullReferenceException($"Domain user with UserId {id} not found.");

    }
}
