using MakerSchedule.Application.DTO.EventTag;
using MediatR;

namespace MakerSchedule.Application.EventTags.Commands;

public class CreateEventTagCommand : IRequest<EventTagDTO>
{
    public string Name { get; } = string.Empty;
    public string Color { get; } = string.Empty;

    public CreateEventTagCommand(CreateEventTagDTO dto)
    {
        Name = dto.Name;
        Color = dto.Color;
    }

}