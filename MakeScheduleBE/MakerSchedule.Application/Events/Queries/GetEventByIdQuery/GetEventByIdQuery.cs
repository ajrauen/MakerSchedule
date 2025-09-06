using MakerSchedule.Application.DTO.Event;

using MediatR;

namespace MakerSchedule.Application.Events.Queries;

public record GetEventByIdQuery(Guid Id) : IRequest<EventDTO>;