using System.Reflection.Emit;

using AutoMapper;

using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.DTO.User;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Aggregates.DomainUser;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.Constants;
using MakerSchedule.Domain.Exceptions;
using MakerSchedule.Domain.ValueObjects;

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
           
           var userRoles = await (
                from du in _context.DomainUsers
                join u in _context.Users on du.UserId equals u.Id
                join ur in _context.UserRoles on u.Id equals ur.UserId into urj
                from ur in urj.DefaultIfEmpty()
                join r in _context.Roles on ur.RoleId equals r.Id into rj
                from r in rj.DefaultIfEmpty()
                select new {
                    DomainUser = du,
                    RoleName = r != null ? r.Name : null
                }
            ).ToListAsync();

            var x = await _context.Roles.ToListAsync();


            var grouped = userRoles
                .GroupBy(x => x.DomainUser.Id)
                .Select(g => new DomainUserListDTO
                {
                    Id = g.Key,
                    FirstName = g.First().DomainUser.FirstName,
                    LastName = g.First().DomainUser.LastName,
                    Roles = g.Where(x => x.RoleName != null).Select(x => x.RoleName).ToArray(),
                    Email = g.First().DomainUser.Email.ToString()
                })
                .ToList();

            return grouped;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetch DomainUser Ids");
            throw new BaseException("Failed to fetch DomainUser IDs", "FETCH_ERROR", 500, ex);
        }
    }

    public async Task<DomainUserDTO> GetDomainUserByIdAsync(Guid id)
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
                Email = DomainUser.Email,
                FirstName = DomainUser.FirstName,
                LastName = DomainUser.LastName,
                PhoneNumber = DomainUser.PhoneNumber,
                Address = DomainUser.Address,
                IsActive = DomainUser.IsActive,

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


    public async Task DeleteDomainUserByIdAsync(Guid id)
    {
        var DomainUser = await _context.DomainUsers.FirstOrDefaultAsync(e => e.Id == id);
        if (DomainUser == null)
        {
            throw new NotFoundException("DomainUser", id);
        }

        var user = await userManager.FindByIdAsync(DomainUser.UserId.ToString());
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
            FirstName = du.FirstName,
            LastName = du.LastName,
            Roles = Array.Empty<string>(),
            Email = du.Email.ToString()
        });
    }


    public async Task<IEnumerable<DomainUserListDTO>> GetAvailableOccurrenceLeadersAsync(string startTime, long duration, List<Guid> currentLeaderIds, Guid? currentOccurrenceId ) 
    {
        ScheduleStart occurrenceStart;
        try
        {
            var parsedDate = DateTimeOffset.Parse(startTime).UtcDateTime;
            _logger.LogInformation("Parsed ISO timestamp {StartTime} to date: {ParsedDate}", startTime, parsedDate);
            occurrenceStart = ScheduleStart.Create(parsedDate);
        }
        catch (ScheduleDateOutOfBoundsException ex)
        {
            throw new BaseException(ex.Message, "@TODO_ERROR_CODE", 400);
        }

        var occurrenceEnd = occurrenceStart.Value.AddMilliseconds(duration);

        var leaderUsers = await userManager.GetUsersInRoleAsync(Roles.Leader);
        var allLeaderIds = leaderUsers.Select(l => l.Id).ToList();

        var domainUsers = await _context.DomainUsers.Include(du => du.User).Where(du => allLeaderIds.Contains(du.UserId)).ToListAsync();
        
        var allLeaders = await _context.OccurrenceLeaders
            .Include(l => l.Occurrence)
            .ThenInclude(o => o.Event) 
            .Where(o => o.Occurrence != null && !o.Occurrence.isDeleted)
            .ToListAsync();

        if (currentOccurrenceId.HasValue && currentOccurrenceId.Value == Guid.Empty)
        {
            currentOccurrenceId = null;
        }

        // For currentLeaderIds, only exclude if double-booked (overlap with another occurrence)
        var doubleBookedLeaderIds = new HashSet<Guid>();
        foreach (var leaderId in currentLeaderIds)
        {
            var leaderOccurrences = allLeaders.Where(l => l.UserId == leaderId).ToList();
            foreach (var occLeader in leaderOccurrences)
            {
                if (currentOccurrenceId.HasValue && occLeader.Occurrence.Id == currentOccurrenceId.Value)
                    continue;
                var occStart = occLeader.Occurrence.ScheduleStart!.Value;
                
                // ✅ Updated: Use Event.Duration instead of Occurrence.Duration
                var occEnd = occStart.AddMinutes(occLeader.Occurrence.Event.Duration.Value);
                
                if (occStart < occurrenceEnd && occEnd > occurrenceStart.Value)
                {
                    doubleBookedLeaderIds.Add(leaderId);
                    break;
                }
            }
        }

        // Find all leaders busy for the requested window (excluding current occurrence)
        var busyLeaders = allLeaders
            .Where(o =>
            {
                if (currentOccurrenceId.HasValue && o.Occurrence.Id == currentOccurrenceId.Value)
                    return false;
                var occStart = o.Occurrence.ScheduleStart!.Value;
                
                // ✅ Updated: Use Event.Duration instead of Occurrence.Duration
                var occEnd = occStart.AddMinutes(o.Occurrence.Event.Duration.Value);
                
                return occStart < occurrenceEnd && occEnd > occurrenceStart.Value;
            })
            .Select(l => l.UserId)
            .Distinct()
            .ToList();

        // Leaders available: not busy, and current leaders only excluded if double-booked
        var availableLeaders = domainUsers.Where(u =>
            (!busyLeaders.Contains(u.Id) || (currentLeaderIds.Contains(u.Id) && !doubleBookedLeaderIds.Contains(u.Id)))
        );

        return availableLeaders.Select(l => new DomainUserListDTO
        {
            Id = l.Id,
            FirstName = l.FirstName,
            LastName = l.LastName,
            Roles = Array.Empty<string>(),
            Email = l.Email.ToString()
        });
    }
}