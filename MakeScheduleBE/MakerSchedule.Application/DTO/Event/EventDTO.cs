using System.ComponentModel.DataAnnotations;

using MakerSchedule.Application.DTO.EventTag;

using MakerSchedule.Application.DTO.Occurrence;

namespace MakerSchedule.Application.DTO.Event;

public class EventDTO
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public required string EventName { get; set; }

    [Required]
    public required string Description { get; set; }
    public required Guid[] EventTagIds { get; set; } = Array.Empty<Guid>();
    public int? Duration { get; set; }
    public string? ThumbnailUrl { get; set; }
    public IEnumerable<OccurrenceDTO> occurrences { get; set; } = new List<OccurrenceDTO>();
    public required int ClassSize { get; set; }
    public decimal? Price { get; set; }

} 


