namespace MakerSchedule.Domain.Aggregates.Event;

using System.ComponentModel.DataAnnotations;
using MakerSchedule.Domain.Aggregates.DomainUser;

public class OccurrenceLeader
{
    [Key]
    public Guid Id { get; set; } =   Guid.NewGuid();
    
    public Guid OccurrenceId { get; set; } =  Guid.NewGuid();
    public Occurrence Occurrence { get; set; } = null!;

    public Guid UserId { get; set; } =  Guid.NewGuid();
    public DomainUser User { get; set; } = null!;

    // Optional metadata
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public string? Role { get; set; } 
} 