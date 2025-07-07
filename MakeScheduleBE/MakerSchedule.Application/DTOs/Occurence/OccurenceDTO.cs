
using MakerSchedule.Application.DTOs.Event;

namespace MakerSchedule.Application.DTOs.Occurence;

public class OccurenceDTO
{
    public int Id { get; set; }

    public int EventId { get; set; }
    public int? Duration { get; set; }
    public ICollection<int> Attendees { get; set; } = Array.Empty<int>();
    public ICollection<int> Leaders { get; set; } = Array.Empty<int>();

    
}