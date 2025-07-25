using MakerSchedule.Application.DTO.Metadata;

namespace MakerSchedule.Application.Interfaces;

public interface IEventsMetadataService
{
    Task<EventsMetadataDTO> GetEventsMetadata();
}

