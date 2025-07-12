namespace MakerSchedule.Domain.Aggregates.Event;

using System.ComponentModel.DataAnnotations;

public class Occurrence
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public DateTime ScheduleStart { get; set; }
    public int? Duration { get; set; }

    // Navigation properties for many-to-many relationships
    public ICollection<OccurrenceAttendee> Attendees { get; set; } = new List<OccurrenceAttendee>();
    public ICollection<OccurrenceLeader> Leaders { get; set; } = new List<OccurrenceLeader>();

    public Occurrence() { }

    public Occurrence(int eventId, OccurrenceInfo info)
    {
        EventId = eventId;
        ScheduleStart = info.ScheduleStart;
        Duration = info.Duration;
        // Note: Attendees and Leaders will be managed through the join entities
    }
} 