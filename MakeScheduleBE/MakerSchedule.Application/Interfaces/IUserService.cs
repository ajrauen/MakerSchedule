using MakerSchedule.Application.DTO.User;
using MakerSchedule.Domain.Aggregates.User;
using Microsoft.AspNetCore.Identity;

namespace MakerSchedule.Application.Interfaces;

public interface IUserService
{
    Task<IdentityResult> UpdateUserAsync(User user);
    Task<IEnumerable<LeaderDTO>> GetAvailableOccurrenceLeaders(string eventId);

}