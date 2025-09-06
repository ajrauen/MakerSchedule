using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.DTO.Occurrence;

using MediatR;

namespace MakerSchedule.Application.Events.Queries;

public record GetOccurrencesByDateQuery(SearchOccurrenceDTO searchDTO) : IRequest<IEnumerable<OccurrenceDTO>>;