using MediatR;
using MakerSchedule.Application.DTO.DomainUser;

namespace MakerSchedule.Application.DomainUsers.Queries;

public class GetAvailableLeadersQuery : IRequest<IEnumerable<DomainUserListDTO>>
{
	public string StartTime { get; set; } = string.Empty;
	public long Duration { get; set; }
	public List<Guid>? CurrentLeaderIds { get; set; }
	public Guid? OccurrenceId { get; set; }

	public GetAvailableLeadersQuery() {}

	public GetAvailableLeadersQuery(string startTime, long duration, List<Guid>? currentLeaderIds, Guid? occurrenceId)
	{
		StartTime = startTime;
		Duration = duration;
		CurrentLeaderIds = currentLeaderIds;
		OccurrenceId = occurrenceId;
	}
}
