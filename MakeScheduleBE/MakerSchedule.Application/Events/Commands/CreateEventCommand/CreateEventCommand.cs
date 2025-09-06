

using MakerSchedule.Application.DTO.Event;
using MediatR;

namespace MakerSchedule.Application.Events.Commands;

public class CreateEventCommand : IRequest<EventDTO>
{
    public CreateEventDTO CreateEventDTO { get; set; } = null!;

    public CreateEventCommand(CreateEventDTO dto)
    {
        CreateEventDTO = dto;
    }
}