

using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.DTO.Occurrence;

using MediatR;

namespace MakerSchedule.Application.Events.Commands;

public class CreateOccurrenceCommand : IRequest<OccurrenceDTO>
{
    public CreateOccurrenceDTO CreateOccurrenceDTO { get; set; } = null!;

    public CreateOccurrenceCommand(CreateOccurrenceDTO dto)
    {
        CreateOccurrenceDTO = dto;
    }
}