using MakerSchedule.Application.DTO.Occurrence;

using MediatR;

public class UpdateOccurrenceCommand : IRequest<OccurrenceDTO>
{
    public UpdateOccurrenceDTO UpdateOccurrenceDTO { get; }

    public UpdateOccurrenceCommand(UpdateOccurrenceDTO updateOccurrenceDTO)
    {
        UpdateOccurrenceDTO = updateOccurrenceDTO;
    }
}
