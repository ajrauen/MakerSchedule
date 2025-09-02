using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Application.DTO.Occurrence;



public class OccurenceDateDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; } = Guid.NewGuid();
    public string EventName { get; set; } = string.Empty;
    public DateTime ScheduleStart { get; set; }
    public ICollection<OccurrenceAttendeeDTO> Attendees { get; set; } = Array.Empty<OccurrenceAttendeeDTO>();
    public ICollection<OccurrenceAttendeeDTO> Leaders { get; set; } = Array.Empty<OccurrenceAttendeeDTO>();
    public OccurrenceStatus Status { get; set; }
} 