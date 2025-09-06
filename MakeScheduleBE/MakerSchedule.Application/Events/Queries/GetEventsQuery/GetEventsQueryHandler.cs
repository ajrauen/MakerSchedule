using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Application.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Events.Queries;

public class GetEventsQueryHandler(ILogger<GetEventsQueryHandler> logger, IApplicationDbContext context) : IRequestHandler<GetEventsQuery, IEnumerable<EventListDTO>>
{
    public async Task<IEnumerable<EventListDTO>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await context.Events.Select(e => new EventListDTO
        {
            Id = e.Id,
            EventName = e.EventName.ToString(),
            Description = e.Description,
            EventTagIds = e.EventTagIds.ToArray(),
            Duration = e.Duration,
            ThumbnailUrl = e.ThumbnailUrl,
            ClassSize = e.ClassSize,
            Price = e.Price
        }).ToListAsync(cancellationToken);

        return events;
    }
}