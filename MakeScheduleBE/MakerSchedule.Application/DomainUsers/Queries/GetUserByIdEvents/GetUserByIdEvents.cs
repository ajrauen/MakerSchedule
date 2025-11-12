using MakerSchedule.Application.DTO.Event;

using MediatR;
namespace MakerSchedule.Application.DomainUsers.Queries;

public record GetUserByIdEventsQuery(Guid Id) : IRequest<List<UserEventDTO>>;