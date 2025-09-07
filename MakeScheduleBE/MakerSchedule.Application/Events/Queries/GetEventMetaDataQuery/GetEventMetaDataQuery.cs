using MakerSchedule.Application.DTO.Metadata;

using MediatR;

namespace MakerSchedule.Application.Events.Queries;

public class GetEventMetaDataQuery : IRequest<EventsMetadataDTO>;