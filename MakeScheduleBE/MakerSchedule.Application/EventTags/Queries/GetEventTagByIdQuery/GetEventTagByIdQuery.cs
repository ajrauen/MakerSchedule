using MediatR;
using MakerSchedule.Application.DTO.EventTag;

namespace MakerSchedule.Application.EventTags.Queries;

public record GetEventTagByIdQuery(Guid Id) : IRequest<EventTagDTO>;