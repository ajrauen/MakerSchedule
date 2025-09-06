using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Events.Commands;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

using MediatR;

using Microsoft.Extensions.Logging;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Exceptions;

public class CreateOccurrenceCommandHandler(IApplicationDbContext context, ILogger<CreateOccurrenceCommandHandler> logger, IImageStorageService imageStorageService, IMediator mediator) : IRequestHandler<CreateOccurrenceCommand, OccurrenceDTO>
{
    private const double RequiredAspectRatio = 4.0 / 3.0;

    public async Task<OccurrenceDTO> Handle(CreateOccurrenceCommand request, CancellationToken cancellationToken)
    {
        var occurrenceDTO = request.CreateOccurrenceDTO;
        var eventEntity = await context.Events.Include(e => e.Occurrences).FirstOrDefaultAsync(e => e.Id == occurrenceDTO.EventId);
        if (eventEntity == null)
            throw new NotFoundException($"Event with id {occurrenceDTO.EventId} not found", occurrenceDTO.EventId);

        OccurrenceInfo info;
        Occurrence newOccurrence;
        try
        {
            var start = occurrenceDTO.ScheduleStart;
            if (start.Kind == DateTimeKind.Local)
                start = start.ToUniversalTime();
            else if (start.Kind == DateTimeKind.Unspecified)
                start = DateTime.SpecifyKind(start, DateTimeKind.Local).ToUniversalTime();
            // Now start is always UTC
            info = new OccurrenceInfo(start);
            newOccurrence = eventEntity.AddOccurrence(info);
            context.Occurrences.Add(newOccurrence);
        }
        catch (ScheduleDateOutOfBoundsException ex)
        {
            logger.LogError("Exception type: {Type}, message: {Message}", ex.GetType().FullName, ex.Message);
            throw new BaseException(ex.Message, "SCHEDULE_START_INVALID", 400);
        }


        await context.SaveChangesAsync();

        var uniqueLeaderIds = occurrenceDTO.Leaders.Distinct().ToList();
        var leaderDTOs = new List<OccurrenceAttendeeDTO>();
        var attendeeDTOs = new List<OccurrenceAttendeeDTO>();

        var allUserIds = uniqueLeaderIds.Concat(occurrenceDTO.Attendees).Distinct().ToList();
        var users = await context.DomainUsers
            .Where(u => allUserIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u);

        foreach (var leaderId in uniqueLeaderIds)
        {
            if (users.TryGetValue(leaderId, out var leader))
            {
                var occurrenceLeader = new OccurrenceLeader
                {
                    OccurrenceId = newOccurrence.Id,
                    UserId = leaderId,
                    AssignedAt = DateTime.UtcNow
                };
                context.OccurrenceLeaders.Add(occurrenceLeader);

                leaderDTOs.Add(new OccurrenceAttendeeDTO
                {
                    Id = leaderId,
                    FirstName = leader.FirstName ?? "",
                    LastName = leader.LastName ?? "",
                    Email = leader.Email?.Value ?? ""
                });
            }
        }

        foreach (var attendeeId in occurrenceDTO.Attendees)
        {
            if (users.TryGetValue(attendeeId, out var attendee))
            {
                var occurrenceAttendee = new OccurrenceAttendee
                {
                    OccurrenceId = newOccurrence.Id,
                    UserId = attendeeId,
                    RegisteredAt = DateTime.UtcNow
                };
                context.OccurrenceAttendees.Add(occurrenceAttendee);

                attendeeDTOs.Add(new OccurrenceAttendeeDTO
                {
                    Id = attendeeId,
                    FirstName = attendee.FirstName ?? "",
                    LastName = attendee.LastName ?? "",
                    Email = attendee.Email?.Value ?? ""
                });
            }
        }

        await context.SaveChangesAsync();
        logger.LogInformation("Successfully created occurrence with {OccurrenceId}", newOccurrence.Id);

        return new OccurrenceDTO
        {
            Id = newOccurrence.Id,
            EventId = newOccurrence.EventId,
            ScheduleStart = DateTime.SpecifyKind(newOccurrence.ScheduleStart?.Value ?? DateTime.MinValue, DateTimeKind.Utc),
            Status = newOccurrence.Status,
            Attendees = attendeeDTOs,
            Leaders = leaderDTOs,
            EventName = eventEntity.EventName.Value,
        };
    }
}