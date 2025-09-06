using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Domain.ValueObjects;
using MakerSchedule.Domain.Aggregates.Event;

public class UpdateOccurrenceCommandHandler(IApplicationDbContext context, ILogger<UpdateOccurrenceCommand> logger, IImageStorageService imageStorageService) : IRequestHandler<UpdateOccurrenceCommand, OccurrenceDTO>
{
    public async Task<OccurrenceDTO> Handle(UpdateOccurrenceCommand request, CancellationToken cancellationToken)
    {
        var occurrenceDTO = request.UpdateOccurrenceDTO;
        var eventEntity = await context.Events.Include(e => e.Occurrences).FirstOrDefaultAsync(e => e.Id == occurrenceDTO.EventId);
        if (eventEntity == null)
            throw new NotFoundException($"Event with id {occurrenceDTO.EventId} not found", occurrenceDTO.EventId);

        var occurrence = await context.Occurrences.FirstOrDefaultAsync(o => o.Id == occurrenceDTO.Id && o.EventId == occurrenceDTO.EventId);
        if (occurrence == null)
            throw new NotFoundException($"Occurrence with id {occurrenceDTO.Id} not found", occurrenceDTO.Id);

        var start = occurrenceDTO.ScheduleStart;
        if (start.Kind == DateTimeKind.Local)
            start = start.ToUniversalTime();
        else if (start.Kind == DateTimeKind.Unspecified)
            start = DateTime.SpecifyKind(start, DateTimeKind.Local).ToUniversalTime();
        occurrence.ScheduleStart = ScheduleStart.Create(start);

        var existingLeaders = context.OccurrenceLeaders.Where(l => l.OccurrenceId == occurrence.Id);
        context.OccurrenceLeaders.RemoveRange(existingLeaders);
        var existingAttendees = context.OccurrenceAttendees.Where(a => a.OccurrenceId == occurrence.Id);
        context.OccurrenceAttendees.RemoveRange(existingAttendees);

        var uniqueLeaderIds = occurrenceDTO.Leaders.Distinct().ToList();
        var uniqueAttendeeIds = occurrenceDTO.Attendees.Distinct().ToList();

        var leaderDTOs = new List<OccurrenceAttendeeDTO>();
        var attendeeDTOs = new List<OccurrenceAttendeeDTO>();


        // Fetch all users at once to avoid multiple DB calls
        var allUserIds = uniqueLeaderIds.Concat(uniqueAttendeeIds).Distinct().ToList();
        var users = await context.DomainUsers
            .Where(u => allUserIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u);

        foreach (var leaderId in uniqueLeaderIds)
        {
            if (users.TryGetValue(leaderId, out var leader))
            {
                var occurrenceLeader = new OccurrenceLeader
                {
                    OccurrenceId = occurrence.Id,
                    UserId = leaderId,
                    AssignedAt = DateTime.UtcNow
                };
                leaderDTOs.Add(new OccurrenceAttendeeDTO
                {
                    Id = leaderId,
                    FirstName = leader.FirstName ?? "",
                    LastName = leader.LastName ?? "",
                    Email = leader.Email?.Value ?? ""
                });
                context.OccurrenceLeaders.Add(occurrenceLeader);
            }
        }

        foreach (var attendeeId in uniqueAttendeeIds)
        {
            if (users.TryGetValue(attendeeId, out var attendee))
            {
                var occurrenceAttendee = new OccurrenceAttendee
                {
                    OccurrenceId = occurrence.Id,
                    UserId = attendeeId,
                    RegisteredAt = DateTime.UtcNow
                };
                attendeeDTOs.Add(new OccurrenceAttendeeDTO
                {
                    Id = attendeeId,
                    FirstName = attendee.FirstName ?? "",
                    LastName = attendee.LastName ?? "",
                    Email = attendee.Email?.Value ?? ""
                });
                context.OccurrenceAttendees.Add(occurrenceAttendee);
            }
        }

        await context.SaveChangesAsync();
        return new OccurrenceDTO
        {
            Attendees = attendeeDTOs,
            Leaders = leaderDTOs,
            Id = occurrence.Id,
            EventId = occurrence.EventId,
            ScheduleStart = DateTime.SpecifyKind(occurrence.ScheduleStart?.Value ?? DateTime.MinValue, DateTimeKind.Utc),
            Status = occurrence.Status,
            EventName = occurrence.Event.EventName.Value,
        };
    }

}