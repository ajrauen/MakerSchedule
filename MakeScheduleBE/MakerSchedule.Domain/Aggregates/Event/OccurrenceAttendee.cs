namespace MakerSchedule.Domain.Aggregates.Event;

using System.ComponentModel.DataAnnotations;
using MakerSchedule.Domain.Aggregates.DomainUser;

public class OccurrenceAttendee
{
    [Key]
    public Guid Id { get; set; } =  Guid.NewGuid();
    
    public Guid OccurrenceId { get; set; } =  Guid.NewGuid();
    public Occurrence Occurrence { get; set; } = null!;

    public Guid UserId { get; set; } =  Guid.NewGuid();
    public DomainUser User { get; set; } = null!;

    // Optional metadata
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public bool Attended { get; set; } = false;

} 