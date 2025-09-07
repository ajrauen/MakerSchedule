using MakerSchedule.Application.DTO.Metadata;
using MakerSchedule.Application.Events.Queries;
using MakerSchedule.Application.Interfaces;

using MediatR;

public class GetEventMetaDataQueryHandler(): IRequestHandler<GetEventMetaDataQuery, EventsMetadataDTO>
{
    
  private static readonly Dictionary<int, string> Durations = new()
    {
        {15 * 60 , "15 minutes" },
        {30 * 60 , "30 minutes" },
        {45 * 60 , "45 minutes" },
        {60 * 60 , "1 hour" },
        {90 * 60 , "1 hour 30 minutes"},
        {120 * 60 , "2 hours" },
        {150 * 60 , "2 hours 30 minutes" },
        {180 * 60 , "3 hours" },
        {210 * 60 , "3 hours 30 minutes" },
        {240 * 60 , "4 hours" },
    };

    public async Task<EventsMetadataDTO> Handle(GetEventMetaDataQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult( new EventsMetadataDTO
        {
            Durations = Durations,
        });
    }
}