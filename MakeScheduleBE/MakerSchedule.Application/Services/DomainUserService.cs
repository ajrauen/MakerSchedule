using AutoMapper;

using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.DomainUser;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.Constants;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class DomainUserService(
    IApplicationDbContext context,
    ILogger<DomainUserService> logger,
    UserManager<User> userManager,
    IMapper mapper) : IDomainUserService
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<DomainUserService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DomainUser>> GetAllDomainUsersWithDetailsAsync()
    {
        try
        {
            return await _context.DomainUsers.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching DomainUsers");
            throw new BaseException("Failed to fetch DomainUsers", "FETCH_ERROR", 500, ex);
        }
    }


    public async Task<IEnumerable<DomainUserListDTO>> GetAllDomainUsersAsync()
    {
        try
        {
            var DomainUsers = await _context.DomainUsers
                            .Include(e => e.User)
                            .ToListAsync();

            var DomainUserDTO = DomainUsers.Select(DomainUser => new DomainUserListDTO
            {
                Id = DomainUser.Id,
                FirstName = DomainUser.User?.FirstName ?? string.Empty,
                LastName = DomainUser.User?.LastName ?? string.Empty,

            }).ToList();


            return DomainUserDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetch DomainUser Ids");
            throw new BaseException("Failed to fetch DomainUser IDs", "FETCH_ERROR", 500, ex);
        }
    }

    public async Task<DomainUserDTO> GetDomainUserByIdAsync(string id)
    {
        try
        {
            var DomainUser = await _context.DomainUsers
                .Include(e => e.User)
                .Include(e => e.OccurrencesLed)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (DomainUser == null)
            {
                throw new NotFoundException("DomainUser", id);
            }
            return new DomainUserDTO
            {
                Id = DomainUser.Id,

                UserId = DomainUser.UserId,
                Email = DomainUser.User?.Email ?? string.Empty,
                FirstName = DomainUser.User?.FirstName ?? string.Empty,
                LastName = DomainUser.User?.LastName ?? string.Empty,
                PhoneNumber = DomainUser.User?.PhoneNumber ?? string.Empty,
                Address = DomainUser.User?.Address ?? string.Empty,
                IsActive = DomainUser.User?.IsActive ?? false,

            };
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching DomainUser by id: {Id}", id);
            throw new BaseException("Failed to fetch DomainUser", "FETCH_ERROR", 500, ex);
        }
    }


    public async Task DeleteDomainUserByIdAsync(string id)
    {
        var DomainUser = await _context.DomainUsers.FirstOrDefaultAsync(e => e.Id == id);
        if (DomainUser == null)
        {
            throw new NotFoundException("DomainUser", id);
        }

        var user = await userManager.FindByIdAsync(DomainUser.UserId);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found.", DomainUser.UserId);
            throw new NotFoundException("User not found", DomainUser.UserId);
        }

        _context.DomainUsers.Remove(DomainUser);
        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new BaseException(
                        message: $"Failed to delete user '{user.Id}'.",
                        errorCode: "USER_DELETION_FAILED",
                        statusCode: 500 // Or another appropriate status code like 400 Bad Request
                    );
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<DomainUserListDTO>> GetAllDomainUsersByRoleAsync(string role)
    {
        var usersInRole = await userManager.GetUsersInRoleAsync(role);
        var userIds = usersInRole.Select(u => u.Id).ToList();

        var domainUsers = await _context.DomainUsers.Include(du => du.User)
            .Where(du => userIds.Contains(du.UserId))
            .ToListAsync();

        return domainUsers.Select(du => new DomainUserListDTO
        {
            Id = du.Id,
            FirstName = du.User?.FirstName ?? string.Empty,
            LastName = du.User?.LastName ?? string.Empty
        });
    }


    public async Task<IEnumerable<LeaderDTO>> GetAvailableOccurrenceLeadersAsync(string occurrenceId)
    {
        var occurrence = await _context.Occurrences.Include(o => o.Event).FirstOrDefaultAsync(o => o.Id == occurrenceId);
        if (occurrence == null)
        {
            throw new NotFoundException("Occurrence not found", occurrenceId);
        }

        var occurrenceStart = occurrence.ScheduleStart ?? DateTime.MinValue;
        var occurrenceEnd = occurrenceStart.Value.AddMinutes(occurrence.Duration ?? 0);

        var leaderUsers = await userManager.GetUsersInRoleAsync(Roles.Leader);
        var leaderIds = leaderUsers.Select(l => l.Id).ToList();

        var domainUsers = await _context.DomainUsers.Include(du => du.User).Where(du => leaderIds.Contains(du.UserId)).ToListAsync();
        // Get all occurrence leaders with their occurrences
        var allLeaders = await _context.OccurrenceLeaders
            .Include(l => l.Occurrence)
            .Where(o => o.Occurrence != null)
            .ToListAsync();

        // Now filter in-memory using C# (not SQL)
        var busyLeaders = allLeaders
            .Where(o =>
                o.Occurrence.ScheduleStart < occurrenceEnd &&
                o.Occurrence.ScheduleStart.Value.AddMilliseconds(o.Occurrence.Duration ?? 0) > occurrenceStart)
            .Select(l => l.UserId)
            .Distinct()
            .ToList();

        var availableLeaders = domainUsers.Where(u => !busyLeaders.Contains(u.Id));

        return availableLeaders.Select(l => new LeaderDTO
        {
            Id = l.Id,
            FirstName = l.User.FirstName,
            LastName = l.User.LastName,
        });
    }
}