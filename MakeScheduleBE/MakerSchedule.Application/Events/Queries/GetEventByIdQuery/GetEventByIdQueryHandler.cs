using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Events.Queries;

public class GetEventByIdQueryHandler(ILogger<GetEventByIdQueryHandler> logger, IApplicationDbContext context) : IRequestHandler<GetEventByIdQuery, EventDTO>
{
    public async Task<EventDTO> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(request.Id), "Event ID cannot be null.");
        }

        var e = await context.Events
            .Include(e => e.Occurrences)
                .ThenInclude(o => o.Attendees)
                    .ThenInclude(a => a.User)
                        .ThenInclude(u => u.User)
            .Include(e => e.Occurrences)
                .ThenInclude(o => o.Leaders)
                    .ThenInclude(l => l.User)
                        .ThenInclude(u => u.User)
            .FirstOrDefaultAsync(e => e.Id == request.Id);
        if(e == null)
        {
            logger.LogWarning($"Event not found: {request.Id}");
            throw new KeyNotFoundException($"Event with ID {request.Id} not found.");
        }

        return new EventDTO
        {
            Id = e.Id,
            EventName = e.EventName.ToString(),
            Description = e.Description,

            Duration = e.Duration,
            ThumbnailUrl = e.ThumbnailUrl,
            EventTagIds = e.EventTagIds.ToArray(),
            ClassSize = e.ClassSize,
            Price = e.Price,
            Occurrences = e.Occurrences
                .Where(o => !o.isDeleted)
                .Select(o => new OccurrenceDTO
                {
                    Id = o.Id,
                    Attendees = o.Attendees.Select(a => new OccurrenceAttendeeDTO
                    {
                        Id = a.UserId,
                        FirstName = a.User?.FirstName ?? "",
                        LastName = a.User?.LastName ?? "",
                        Email = a.User?.Email?.Value ?? ""
                    }).ToList(),
                    EventId = o.EventId,
                    Leaders = o.Leaders.Select(l => new OccurrenceAttendeeDTO
                    {
                        Id = l.UserId,
                        FirstName = l.User?.FirstName ?? "",
                        LastName = l.User?.LastName ?? "",
                        Email = l.User?.Email?.Value ?? ""
                    }).ToList(),
                    ScheduleStart = o.ScheduleStart != null ? DateTime.SpecifyKind(o.ScheduleStart.Value, DateTimeKind.Utc) : DateTime.MinValue,
                    Status = o.Status,
                    EventName = o.Event.EventName.Value,
                })
            
        };
    }
}