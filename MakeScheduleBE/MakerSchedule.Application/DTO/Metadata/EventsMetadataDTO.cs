using MakerSchedule.Application.DTO.EventType;

namespace MakerSchedule.Application.DTO.Metadata;

public class EventsMetadataDTO
{
    public Dictionary<int, string> Durations { get; set; } = new();
    public EventTypeDTO[] EventTypes { get; set; } = Array.Empty<EventTypeDTO>();
}