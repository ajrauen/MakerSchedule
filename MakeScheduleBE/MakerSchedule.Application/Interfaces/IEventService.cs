using MakerSchedule.Application.DTOs.Event;
using MakerSchedule.Domain.Entities;

namespace MakerSchedule.Application.Services
{
    public interface IEventService
    {
        Task<IEnumerable<EventListDTO>> GetAllEventsAsync();
        Task<EventDTO> GetEventAsync(int eventId);
        Task<int> CreateEventAsync(CreateEventDTO eventDTO);
        
    }
}
