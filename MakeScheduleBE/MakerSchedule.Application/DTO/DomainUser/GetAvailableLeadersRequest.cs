namespace MakerSchedule.Application.DTO.DomainUser;

public class GetAvailableLeadersRequest
{
    public string StartTime { get; set; } = string.Empty;
    public long Duration { get; set; }
    public List<Guid>? CurrentLeaderIds { get; set; }
    public Guid? OccurrenceId { get; set; }
} 