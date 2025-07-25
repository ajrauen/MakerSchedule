using MakerSchedule.Application.DTO.Event;
using MakerSchedule.Application.DTO.Occurrence;

namespace MakerSchedule.Application.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventListDTO>> GetAllEventsAsync();
    Task<EventDTO> GetEventAsync(Guid eventId);
    Task<Guid> CreateEventAsync(CreateEventDTO eventDTO);
    Task<Guid> PatchEventAsync(Guid eventId, PatchEventDTO eventDTO);
    Task<bool> DeleteEventAsync(Guid eventId);
    Task<Guid> CreateOccurrenceAsync(CreateOccurenceDTO occurrenceDTO);
    Task<bool> UpdateOccuranceAsync(UpdateOccurenceDTO updateOccurenceDTO);
    Task<bool> DeleteOccuranceAsync(Guid occurrenceGUID);

}
