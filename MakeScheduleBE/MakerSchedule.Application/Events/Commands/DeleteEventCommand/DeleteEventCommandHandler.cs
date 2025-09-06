using MakerSchedule.Application.Events.Commands;
using MakerSchedule.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

using MediatR;

public class DeleteEventCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteEventCommand, bool>
{
    public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
       var e = await context.Events.FindAsync(request.EventId);
        if (e == null) return false;
        context.Events.Remove(e);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}   