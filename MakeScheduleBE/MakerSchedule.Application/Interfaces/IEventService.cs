using MakerSchedule.Application.DTO.Event;

namespace MakerSchedule.Application.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventListDTO>> GetAllEventsAsync();
    Task<EventDTO> GetEventAsync(int eventId);
    Task<int> CreateEventAsync(CreateEventDTO eventDTO);
    Task<bool> DeleteEventAsync(int eventId);
}
