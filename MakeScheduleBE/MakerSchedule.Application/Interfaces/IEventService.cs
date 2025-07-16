using MakerSchedule.Application.DTO.Event;

namespace MakerSchedule.Application.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventListDTO>> GetAllEventsAsync();
    Task<EventDTO> GetEventAsync(string eventId);
    Task<string> CreateEventAsync(CreateEventDTO eventDTO);
    Task<bool> DeleteEventAsync(string eventId);

}
