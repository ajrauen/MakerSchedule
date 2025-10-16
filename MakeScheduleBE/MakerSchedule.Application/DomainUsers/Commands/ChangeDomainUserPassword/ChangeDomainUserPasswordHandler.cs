using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.User;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MakerSchedule.Application.DomainUsers.Commands;

public class ChangeDomainUserPasswordHandler(
    IApplicationDbContext context,
    UserManager<User> userManager) : IRequestHandler<ChangeDomainUserPassword, bool>
{

    public async Task<bool> Handle(ChangeDomainUserPassword request, CancellationToken cancellationToken)
    {
        var id = request.id;
        var currentPassword = request.currentPassword;
        var newPassword = request.newPassword;

        var domainUser = await context.DomainUsers.Include(du => du.User).FirstOrDefaultAsync(du => du.Id == id);
        if (domainUser == null) throw new NullReferenceException($"Domain user with Id {id} not found.");

        var user = await userManager.FindByIdAsync(domainUser.UserId.ToString());
        if (user == null) throw new NullReferenceException($"User with Id {domainUser.UserId} not found.");

        var passwordChangeResult = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!passwordChangeResult.Succeeded)
        {
            // Don't reveal which part failed for security - generic message
            throw new InvalidOperationException("Unable to change password. Please verify your current password is correct and the new password meets all requirements.");
        }

        return true;
    }

}