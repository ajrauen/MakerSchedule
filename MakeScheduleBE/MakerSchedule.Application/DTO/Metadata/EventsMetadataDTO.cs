namespace MakerSchedule.Application.DTO.Metadata;

public class EventsMetadataDTO
{
    public Dictionary<int, string> Durations { get; set; } = new();
    public string[] EventTypes { get; set; } = Array.Empty<string>();
}