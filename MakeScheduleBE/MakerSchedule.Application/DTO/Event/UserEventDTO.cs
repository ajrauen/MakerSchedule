namespace MakerSchedule.Application.DTO.Event;

public class UserEventDTO
{
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Occurrence-specific data
    public Guid OccurrenceId { get; set; }
    public DateTime OccurrenceStartTime { get; set; }
    public DateTime OccurrenceEndTime { get; set; }

    // Registration-specific data
    public DateTime RegisteredAt { get; set; }
    public bool Attended { get; set; }
    public int Duration { get; set; }
}