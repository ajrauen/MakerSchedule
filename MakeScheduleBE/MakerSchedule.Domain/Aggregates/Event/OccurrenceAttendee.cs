namespace MakerSchedule.Domain.Aggregates.Event;

using System.ComponentModel.DataAnnotations;
using MakerSchedule.Domain.Aggregates.DomainUser;

public class OccurrenceAttendee
{
    [Key]
    public string Id { get; set; }
    
    public string OccurrenceId { get; set; } = string.Empty;
    public Occurrence Occurrence { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public DomainUser User { get; set; } = null!;

    // Optional metadata
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public bool Attended { get; set; } = false;

} 