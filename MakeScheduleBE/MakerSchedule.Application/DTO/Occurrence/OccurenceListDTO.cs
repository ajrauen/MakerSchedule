namespace MakerSchedule.Application.DTO.Occurrence;

public class OccurenceListDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long ScheduleStart { get; set; }
}