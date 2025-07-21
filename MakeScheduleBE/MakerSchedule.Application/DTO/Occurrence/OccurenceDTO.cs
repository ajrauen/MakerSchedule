using System.Text.Json.Serialization;

namespace MakerSchedule.Application.DTO.Occurrence;



public class OccurenceDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; } = Guid.NewGuid();
    public int? Duration { get; set; }
    public DateTime ScheduleStart { get; set; }
    public ICollection<OccurrenceUserDTO> Attendees { get; set; } = Array.Empty<OccurrenceUserDTO>();
    public ICollection<OccurrenceUserDTO> Leaders { get; set; } = Array.Empty<OccurrenceUserDTO>();
    public OccurrenceStatus Status { get; set; }
} 