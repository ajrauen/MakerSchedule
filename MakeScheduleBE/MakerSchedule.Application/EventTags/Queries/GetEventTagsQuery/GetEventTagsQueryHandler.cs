
using MediatR;


using MakerSchedule.Application.DTO.EventTag;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MakerSchedule.Application.Interfaces;
namespace MakerSchedule.Application.EventTags.Queries;

public class GetEventTagsQueryHandler(ILogger<GetEventTagsQueryHandler> logger, IApplicationDbContext context ) : IRequestHandler<GetEventTagsQuery, IEnumerable<EventTagDTO>>
{

    public async Task<IEnumerable<EventTagDTO>> Handle(GetEventTagsQuery request, CancellationToken cancellationToken)
    {

        var eventTags = await context.EventTags.Select(t => new EventTagDTO
        {
            Id = t.Id,
            Color = t.Color,
            Name = t.Name.Value,
            EventIds = context.Events.Where(e => e.EventTagIds.Contains(t.Id)).Select(e => e.Id).ToList(),
        }).ToListAsync();

        return eventTags;
    }

}
