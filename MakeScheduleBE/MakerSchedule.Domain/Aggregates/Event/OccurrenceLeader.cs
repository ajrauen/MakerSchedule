namespace MakerSchedule.Domain.Aggregates.Event;

using System.ComponentModel.DataAnnotations;
using MakerSchedule.Domain.Aggregates.DomainUser;

public class OccurrenceLeader
{
    [Key]
    public int Id { get; set; }
    
    public int OccurrenceId { get; set; }
    public Occurrence Occurrence { get; set; } = null!;

    public int UserId { get; set; }
    public DomainUser User { get; set; } = null!;

    // Optional metadata
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public string? Role { get; set; } // e.g., "Lead Instructor", "Assistant", etc.
} 