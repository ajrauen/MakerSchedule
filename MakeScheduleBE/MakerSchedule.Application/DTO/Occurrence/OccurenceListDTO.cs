using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Application.DTO.Occurrence;

public class OccurenceListDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ScheduleStart { get; set; } = string.Empty;
    public OccurrenceStatus Status { get; set; }
} 