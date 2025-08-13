namespace MakerSchedule.Application.DTO.Occurrence;

public class CreateOccurrenceDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ICollection<Guid> Attendees { get; set; } = Array.Empty<Guid>();
    public ICollection<Guid> Leaders { get; set; } = Array.Empty<Guid>();
    public DateTime ScheduleStart { get; set; }
    public Guid EventId { get; set; } =  Guid.NewGuid();
} 