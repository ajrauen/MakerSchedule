namespace MakerSchedule.Domain.Aggregates.Event;

using System.ComponentModel.DataAnnotations;
using MakerSchedule.Domain.Aggregates.DomainUser;

public class OccurrenceAttendee
{
    [Key]
    public int Id { get; set; }
    
    public int OccurrenceId { get; set; }
    public Occurrence Occurrence { get; set; } = null!;

    public int UserId { get; set; }
    public DomainUser User { get; set; } = null!;

    // Optional metadata
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public bool Attended { get; set; } = false;

} 