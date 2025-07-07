using MakerSchedule.Application.DTOs.Occurence;
using MakerSchedule.Domain.Entities;

namespace MakerSchedule.Application.Interfaces;

public interface IOccurrenceService
{
    Task<IEnumerable<OccurenceListDTO>> GetAllOccurrencesAsync();
    Task<OccurenceDTO> GetOccurrenceByIdAsync(int id);
    Task<int> CreateOccurrenceAsync(CreateOccurenceDTO occurance);

  
    // Task<bool> UpdateOccurrenceAsync(Occurrence occurance);
    // Task<bool> DeleteOccurrenceAsync(int id);
} 