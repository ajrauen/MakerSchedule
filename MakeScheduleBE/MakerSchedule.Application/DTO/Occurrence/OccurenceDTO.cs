
namespace MakerSchedule.Application.DTO.Occurrence;

public class OccurenceDTO
{
    public string Id { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public int? Duration { get; set; }
    public ICollection<string> Attendees { get; set; } = Array.Empty<string>();
    public ICollection<string> Leaders { get; set; } = Array.Empty<string>();
}