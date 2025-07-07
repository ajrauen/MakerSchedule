using MakerSchedule.Application.DTOs.User;
using MakerSchedule.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Interfaces;

public interface IUserService
{
    Task<IdentityResult> UpdateUserAsync(User user);
}