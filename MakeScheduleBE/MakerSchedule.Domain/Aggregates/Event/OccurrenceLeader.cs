namespace MakerSchedule.Domain.Aggregates.Event;

using System.ComponentModel.DataAnnotations;
using MakerSchedule.Domain.Aggregates.DomainUser;

public class OccurrenceLeader
{
    [Key]
    public string Id { get; set; } =  string.Empty;
    
    public string OccurrenceId { get; set; } = string.Empty;
    public Occurrence Occurrence { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public DomainUser User { get; set; } = null!;

    // Optional metadata
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public string? Role { get; set; } 
} 