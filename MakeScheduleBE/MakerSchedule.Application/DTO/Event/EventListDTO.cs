using MakerSchedule.Application.DTO.EventType;

namespace MakerSchedule.Application.DTO.Event;

public class EventListDTO
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public required string EventName { get; set; }
    public string Description { get; set; } = String.Empty;
    public required EventTypeDTO EventType { get; set; }
    public int? Duration { get; set; }
    public string? ThumbnailUrl { get; set; }
}