
namespace MakerSchedule.Application.DTO.Occurrence;

public class CreateOccurenceDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ICollection<Guid> Attendees { get; set; } = Array.Empty<Guid>();
    public ICollection<Guid> Leaders { get; set; } = Array.Empty<Guid>();
    public long ScheduleStart { get; set; }
    public int Duration { get; set; }
    public Guid EventId { get; set; } =  Guid.NewGuid();
}