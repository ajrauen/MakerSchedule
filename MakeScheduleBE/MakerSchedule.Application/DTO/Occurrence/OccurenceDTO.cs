
namespace MakerSchedule.Application.DTO.Occurrence;

public class OccurenceDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; } =  Guid.NewGuid();
    public int? Duration { get; set; }  
    public long ScheduleStart { get; set; }
    public ICollection<string> Attendees { get; set; } = Array.Empty<string>();
    public ICollection<string> Leaders { get; set; } = Array.Empty<string>();

    
}