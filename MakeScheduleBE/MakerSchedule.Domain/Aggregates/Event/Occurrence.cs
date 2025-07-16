namespace MakerSchedule.Domain.Aggregates.Event;

using System.ComponentModel.DataAnnotations;

using MakerSchedule.Domain.ValueObjects;

public class Occurrence
{
    [Key]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string EventId { get; set; } = string.Empty;
    public Event Event { get; set; } = null!;
    public ScheduleStart? ScheduleStart { get; set; }
    public int? Duration { get; set; }

    // Navigation properties for many-to-many relationships
    public ICollection<OccurrenceAttendee> Attendees { get; set; } = new List<OccurrenceAttendee>();
    public ICollection<OccurrenceLeader> Leaders { get; set; } = new List<OccurrenceLeader>();

    public Occurrence() { }

    public Occurrence(string eventId, OccurrenceInfo info)
    {
        EventId = eventId;
        ScheduleStart = new ScheduleStart(info.ScheduleStart);
        Duration = info.Duration;
        // Note: Attendees and Leaders will be managed through the join entities
    }
} 