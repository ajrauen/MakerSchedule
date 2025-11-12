using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Application.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace MakerSchedule.Application.DomainUsers.Queries;

public class GetUserByIdEventsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetUserByIdEventsQuery, List<UserEventDTO>>
{
    public async Task<List<UserEventDTO>> Handle(GetUserByIdEventsQuery request, CancellationToken cancellationToken)
    {
        var user = await context.DomainUsers
            .Where(du => du.Id == request.Id)
            .Include(du => du.OccurrenceRegistrations)
            .ThenInclude(or => or.Occurrence)
            .ThenInclude(o => o.Event)
            .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("User not found with the given ID.", request.Id.ToString());
            }
            var userEvents = user.OccurrenceRegistrations

                .Select(or => or.Occurrence)
                .Where(o => o != null && !o.isDeleted && o.ScheduleStart?.Value != null && o.Event.Duration != null)
                .Select(o => new UserEventDTO
                {
                    EventId = o.Event.Id,
                    Description = o.Event.Description,
                    OccurrenceId = o.Id,
                    OccurrenceStartTime = o.ScheduleStart!.Value,
                    OccurrenceEndTime = o.ScheduleStart!.Value.AddMinutes(o.Event.Duration?.Value ?? 0),
                    RegisteredAt = user.OccurrenceRegistrations
                        .FirstOrDefault(or => or.OccurrenceId == o.Id)?.RegisteredAt ?? DateTime.MinValue,
                    Attended = user.OccurrenceRegistrations
                        .FirstOrDefault(or => or.OccurrenceId == o.Id)?.Attended ?? false,
                    EventName = o.Event.EventName.Value,
                    Duration = o.Event.Duration?.Value ?? 0
                })
                .ToList();

            if (!userEvents.Any())
            {
                return new List<UserEventDTO>();
            }

        return userEvents;
        }
    }
