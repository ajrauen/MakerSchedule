using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Application.DTO.Occurrence;



public class OccurenceDateDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; } = Guid.NewGuid();
    public string EventName { get; set; } = string.Empty;
    public DateTime ScheduleStart { get; set; }
    public string EventType { get; set; } = string.Empty;
    public ICollection<OccurrenceUserDTO> Attendees { get; set; } = Array.Empty<OccurrenceUserDTO>();
    public ICollection<OccurrenceUserDTO> Leaders { get; set; } = Array.Empty<OccurrenceUserDTO>();
    public OccurrenceStatus Status { get; set; }
} 