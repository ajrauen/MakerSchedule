
namespace MakerSchedule.Application.DTO.Occurrence;

public class CreateOccurenceDTO
{
    public string Id { get; set; } = string.Empty;
    public ICollection<string> Attendees { get; set; } = Array.Empty<string>();
    public ICollection<string> Leaders { get; set; } = Array.Empty<string>();
    public long ScheduleStart { get; set; }
    public int Duration { get; set; }
    public string EventId { get; set; } = string.Empty;
}