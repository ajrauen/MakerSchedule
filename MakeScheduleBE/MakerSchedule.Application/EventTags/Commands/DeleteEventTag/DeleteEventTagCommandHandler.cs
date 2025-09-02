
using MakerSchedule.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.EventTags.Commands;

public class DeleteEventTagCommandHandler(IApplicationDbContext context, ILogger<DeleteEventTagCommandHandler> logger) 
    : IRequestHandler<DeleteEventTagCommand, bool>
{
    public async Task<bool> Handle(DeleteEventTagCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Event tag ID cannot be empty.", nameof(request.Id));
        }

        var result = await context.EventTags.Where(et => et.Id == request.Id).ExecuteDeleteAsync(cancellationToken);

        if (result == 0)
        {
            logger.LogWarning("Event tag not found: {EventTagId}", request.Id);
        }

        return true;
    }
}
