using System.ComponentModel.DataAnnotations;
using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Domain.Entities;

public class Event
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string EventName { get; set; }
    [Required]
    public required string Description { get; set; }
    public EventTypeEnum EventType { get; set; }
    public int Duration { get; set; } // Default duration in minutes (or ms)
    // Navigation property for Occurrences
    public ICollection<Occurrence> Occurrences { get; set; } = new List<Occurrence>();
}