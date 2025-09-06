using MakerSchedule.Application.DTO.EventTag;
using MakerSchedule.Application.Interfaces;
using MakerSchedule.Domain.Entities;

using MediatR;

using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.EventTags.Commands;

public class CreateEventTagCommandHandler(IApplicationDbContext context, ILogger<CreateEventTagCommandHandler> logger) : IRequestHandler<CreateEventTagCommand, EventTagDTO>
{
    public async Task<EventTagDTO> Handle(CreateEventTagCommand request, CancellationToken cancellationToken)
    {
        var eventTag = new EventTag
        {
            Name = request.Name,
            Color = request.Color
        };

        context.EventTags.Add(eventTag);
        await context.SaveChangesAsync();

        logger.LogInformation($"Event tag created: {eventTag.Id}");

        return new EventTagDTO
        {
            Id = eventTag.Id,
            Name = eventTag.Name.Value,
            Color = eventTag.Color
        };
    }
}
