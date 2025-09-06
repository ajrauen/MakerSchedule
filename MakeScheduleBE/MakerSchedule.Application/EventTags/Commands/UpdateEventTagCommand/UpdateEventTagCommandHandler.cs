using MakerSchedule.Application.DTO.EventTag;
using MakerSchedule.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.EventTags.Commands;

public class UpdateEventTagCommandHandler(IApplicationDbContext context, ILogger<UpdateEventTagCommandHandler> logger) 
    : IRequestHandler<UpdateEventTagCommand, EventTagDTO>
{
    public async Task<EventTagDTO> Handle(UpdateEventTagCommand request, CancellationToken cancellationToken)
    {
        var eventTag = await context.EventTags.FirstOrDefaultAsync(et => et.Id == request.Id, cancellationToken);

        if (eventTag == null)
        {
            logger.LogWarning("Event tag not found: {EventTagId}", request.Id);
            throw new KeyNotFoundException($"Event tag with ID {request.Id} not found.");
        }

        if (request.eventTagDTO.Name != null)
        {
            eventTag.Name = request.eventTagDTO.Name;
        }

        if (request.eventTagDTO.Color != null)
        {
            eventTag.Color = request.eventTagDTO.Color;
        }

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Event tag updated: {EventTagId}", eventTag.Id);

        return new EventTagDTO
        {
            Id = eventTag.Id,
            Name = eventTag.Name.Value,
            Color = eventTag.Color
        };
    }
}
