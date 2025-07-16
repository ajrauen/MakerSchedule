using MakerSchedule.Application.DTO.Occurrence;

namespace MakerSchedule.Application.Interfaces;

public interface IOccurrenceService
{
    Task<IEnumerable<OccurenceListDTO>> GetAllOccurrencesAsync();
    Task<OccurenceDTO> GetOccurrenceByIdAsync(string id);
    Task<string> CreateOccurrenceAsync(CreateOccurenceDTO occurrenceDTO);

  
} 