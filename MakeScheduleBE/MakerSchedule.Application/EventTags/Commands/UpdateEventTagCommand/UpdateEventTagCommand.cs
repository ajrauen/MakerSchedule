using MakerSchedule.Application.DTO.EventTag;
using MediatR;

namespace MakerSchedule.Application.EventTags.Commands;

public record UpdateEventTagCommand(Guid Id, PatchEventTagDTO eventTagDTO) : IRequest<EventTagDTO>;
