using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Application.DTO.Event;

public class EventListDTO
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public required string EventName { get; set; }
    public string Description { get; set; } = "";
    public EventTypeEnum EventType { get; set; }
    public int? Duration { get; set; }
    public string? FileUrl { get; set; }
}