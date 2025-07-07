using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Domain.Entities;

public class EventDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public required string EventName { get; set; }

    [Required]
    public required string Description { get; set; }

    public ICollection<int> Attendees { get; set; } = Array.Empty<int>();
    public ICollection<int> Leaders { get; set; } = Array.Empty<int>();
    public long ScheduleStart { get; set; }
    public int Duration { get; set; }
}