using MakerSchedule.Application.DTO.EventTag;

namespace MakerSchedule.Application.Interfaces;

public interface IEventTagService
{
    Task<EventTagDTO> CreateEventTagAsync(CreateEventTagDTO eventTagDto);
    Task<IEnumerable<EventTagDTO>> GetEventTagsAsync();
    Task<EventTagDTO> GetEventTagByIdAsync(Guid eventTagId);
    Task<EventTagDTO> PatchEventTagAsync(Guid eventTagId, PatchEventTagDTO eventTagDto);
    Task<bool> DeleteEventTagAsync(Guid eventTagId);
}