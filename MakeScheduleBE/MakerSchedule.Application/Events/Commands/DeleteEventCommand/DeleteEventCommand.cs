using MediatR;

namespace MakerSchedule.Application.Events.Commands;

public class DeleteEventCommand : IRequest<bool>
{
    public Guid EventId { get; set; }

    public DeleteEventCommand(Guid eventId)
    {
        EventId = eventId;
    }
}