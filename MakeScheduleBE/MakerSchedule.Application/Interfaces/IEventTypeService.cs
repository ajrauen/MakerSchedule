using MakerSchedule.Application.DTO.EventType;

namespace MakerSchedule.Application.Interfaces;

public interface IEventTypeService
{
    
    Task<IEnumerable<EventTypeDTO>> GetAllEventTypesAsync();
    Task<Guid> CreateEventTypeAsync(CreateEventTypeDTO eventTypeDTO);
    Task<bool> DeleteEventTypeAsync(Guid eventTypeId);
}