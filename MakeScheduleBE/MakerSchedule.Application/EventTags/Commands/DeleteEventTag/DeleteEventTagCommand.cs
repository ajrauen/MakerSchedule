using MediatR;

namespace MakerSchedule.Application.EventTags.Commands;

public record DeleteEventTagCommand(Guid Id) : IRequest<bool>;
