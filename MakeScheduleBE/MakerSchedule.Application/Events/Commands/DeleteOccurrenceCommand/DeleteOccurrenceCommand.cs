using MediatR;

namespace MakerSchedule.Application.Events.Commands;

public class DeleteOccurrenceCommand : IRequest<bool>
{
    public Guid OccurrenceId { get; set; }

    public DeleteOccurrenceCommand(Guid occurrenceId)
    {
        OccurrenceId = occurrenceId;
    }
}