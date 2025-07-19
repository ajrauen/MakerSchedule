namespace MakerSchedule.Application.DTOs.Event;

public class EventSummaryDTO
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public string EventName { get; set; } = string.Empty;
} 