
using MakerSchedule.Application.DTO.Event;

namespace MakerSchedule.Application.DTO.Occurrence;

public class OccurenceDTO
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int? Duration { get; set; }
    public ICollection<int> Attendees { get; set; } = Array.Empty<int>();
    public ICollection<int> Leaders { get; set; } = Array.Empty<int>();
}