namespace MakerSchedule.Domain.Aggregates.Event;

using System.ComponentModel.DataAnnotations;

using MakerSchedule.Domain.ValueObjects;

public class Occurrence
{
    [Key]
    public Guid Id { get; set; } =  Guid.NewGuid();
    [Required]
    public Guid EventId { get; set; }
    public Event Event { get; set; } = null!;
    public ScheduleStart? ScheduleStart { get; set; }
    public int? Duration { get; set; }

    // Navigation properties for many-to-many relationships
    public ICollection<OccurrenceAttendee> Attendees { get; set; } = new List<OccurrenceAttendee>();
    public ICollection<OccurrenceLeader> Leaders { get; set; } = new List<OccurrenceLeader>();

    public Occurrence() { }

    public Occurrence(Guid eventId, OccurrenceInfo info)
    {
        EventId = eventId;
        ScheduleStart = new ScheduleStart(info.ScheduleStart);
        Duration = info.Duration;
    }
} 