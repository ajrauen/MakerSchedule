using MediatR;
using MakerSchedule.Application.DTO.EventTag;

namespace MakerSchedule.Application.EventTags.Queries;

public class GetEventTagByIdQuery : IRequest<EventTagDTO>
{
	public Guid Id { get; }

	public GetEventTagByIdQuery(Guid id)
	{
		Id = id;
	}
}
