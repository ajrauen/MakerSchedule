using MakerSchedule.Application.DTO.EventType;

namespace MakerSchedule.Application.Interfaces;

public interface IEventTypeService
{
    
    Task<IEnumerable<EventTypeDTO>> GetAllEventTypesAsync();
    Task<EventTypeDTO> CreateEventTypeAsync(CreateEventTypeDTO eventTypeDTO);
    Task<bool> DeleteEventTypeAsync(Guid eventTypeId);
    Task<EventTypeDTO> PatchEventTypeAsync(Guid eventTypeId, PatchEventTypeDTO eventTypeDTO);
}