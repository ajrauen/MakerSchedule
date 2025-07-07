using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Application.DTOs.Event;

public class EventListDTO
{
    public int Id { get; set; }
    public required string EventName { get; set; }
    public string Description { get; set; } = "";
    public EventTypeEnum EventType { get; set; }
    public int Duration { get; set; }
}