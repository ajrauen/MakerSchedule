using MakerSchedule.Domain.Constants;

using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.DTO.DomainUser;

public class UpdateUserRoleRequest
{

    public Guid? UserId { get; set; }
    public string Role { get; set; } = Roles.Customer;
} 