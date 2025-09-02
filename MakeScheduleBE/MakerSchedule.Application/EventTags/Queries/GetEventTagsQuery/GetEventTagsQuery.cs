using MediatR;
using MakerSchedule.Application.DTO.EventTag;

namespace MakerSchedule.Application.EventTags.Queries;

public class GetEventTagsQuery : IRequest<IEnumerable<EventTagDTO>>
{
	public GetEventTagsQuery() {}
   
}
