
namespace MakerSchedule.Application.DTOs.Occurence;
public class CreateOccurenceDTO
{
    public int Id { get; set; }
    public ICollection<int> Attendees { get; set; } = Array.Empty<int>();
    public ICollection<int> Leaders { get; set; } = Array.Empty<int>();
    public long ScheduleStart { get; set; }
    public int Duration { get; set; }
    public int EventId { get; set; }

};