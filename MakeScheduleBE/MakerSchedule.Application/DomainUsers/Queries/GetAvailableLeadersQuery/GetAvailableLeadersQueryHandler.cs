
using MediatR;
using MakerSchedule.Application.DTO.DomainUser;
using MakerSchedule.Application.Interfaces;
using Microsoft.Extensions.Logging;
using MakerSchedule.Domain.ValueObjects;
using MakerSchedule.Domain.Exceptions;
using MakerSchedule.Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace MakerSchedule.Application.DomainUsers.Queries;

public class GetAvailableLeadersQueryHandler(ILogger<GetAvailableLeadersQueryHandler> logger, IApplicationDbContext context, UserManager<User> userManager) : IRequestHandler<GetAvailableLeadersQuery, IEnumerable<DomainUserListDTO>>
{

    public async Task<IEnumerable<DomainUserListDTO>> Handle(GetAvailableLeadersQuery request, CancellationToken cancellationToken)
    {
        var startTime = request.StartTime;
        var duration = request.Duration;
        var currentLeaderIds = request.CurrentLeaderIds;
        var currentOccurrenceId = request.OccurrenceId;


        ScheduleStart occurrenceStart;
        try
        {
            var parsedDate = DateTimeOffset.Parse(startTime).UtcDateTime;
            logger.LogInformation("Parsed ISO timestamp {StartTime} to date: {ParsedDate}", startTime, parsedDate);
            occurrenceStart = ScheduleStart.Create(parsedDate);
        }
        catch (ScheduleDateOutOfBoundsException ex)
        {
            throw new BaseException(ex.Message, "@TODO_ERROR_CODE", 400);
        }

        var occurrenceEnd = occurrenceStart.Value.AddMinutes(duration);

        var leaderUsers = await userManager.GetUsersInRoleAsync(Roles.Leader);
        var allLeaderIds = leaderUsers.Select(l => l.Id).ToList();

        var domainUsers = await context.DomainUsers.Include(du => du.User).Where(du => allLeaderIds.Contains(du.UserId)).ToListAsync();

        var allLeaders = await context.OccurrenceLeaders
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

                var occEnd = occStart.AddMinutes(duration);

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

                var occEnd = occStart.AddMinutes(duration);

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
