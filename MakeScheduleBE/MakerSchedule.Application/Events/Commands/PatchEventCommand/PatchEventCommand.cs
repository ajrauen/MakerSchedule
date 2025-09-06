using MakerSchedule.Application.DTO.Event;

using MediatR;

namespace MakerSchedule.Application.Events.Commands;

public class PatchEventCommand : IRequest<EventDTO>
{
    public Guid EventId { get; set; }
    public PatchEventDTO EventDTO { get; set; }

    public PatchEventCommand(Guid eventId, PatchEventDTO eventDTO)
    {
        EventId = eventId;
        EventDTO = eventDTO;
    }
}