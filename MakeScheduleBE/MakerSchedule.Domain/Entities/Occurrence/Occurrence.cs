using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Domain.Entities;

public class Occurrence
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public DateTime ScheduleStart { get; set; }
    public int? Duration { get; set; }
    public ICollection<int> Attendees { get; set; } = Array.Empty<int>();
    public ICollection<int> Leaders { get; set; } = Array.Empty<int>();
}