
using MediatR;
using MakerSchedule.Application.Interfaces;
using Microsoft.Extensions.Logging;

using MakerSchedule.Application.DTO.EventTag;
using Microsoft.EntityFrameworkCore;
namespace MakerSchedule.Application.EventTags.Queries;

public class GetEventTagByIdQueryHandler(ILogger<GetEventTagByIdQueryHandler> logger, IApplicationDbContext context ) : IRequestHandler<GetEventTagByIdQuery, EventTagDTO>
{

    public async Task<EventTagDTO> Handle(GetEventTagByIdQuery request, CancellationToken cancellationToken)
    {

        var eventTag = await context.EventTags
            .Where(t => t.Id == request.Id)
            .Select(t => new EventTagDTO
            {
                Id = t.Id,
                Color = t.Color,
                Name = t.Name.Value,
                EventIds = context.Events.Where(e => e.EventTagIds.Contains(t.Id)).Select(e => e.Id).ToList(),
            }).FirstOrDefaultAsync();

        if(eventTag == null)
        {
            logger.LogWarning($"Event tag not found: {request.Id}");
            throw new KeyNotFoundException($"Event tag with ID {request.Id} not found.");
        }

        return eventTag;
    }

}
