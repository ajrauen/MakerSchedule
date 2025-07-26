using System.ComponentModel.DataAnnotations;

using MakerSchedule.Application.DTO.Occurrence;
using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Application.DTO.Event;

public class EventDTO
{
    [Required]
    public Guid Id { get; set; } =  Guid.NewGuid();
    [Required]
    public required string EventName { get; set; }

    [Required]
    public required string Description { get; set; }

    public required string EventType { get; set; }

    public int? Duration { get; set; }
    public string? ThumbnailUrl { get; set; }
    public IEnumerable<OccurenceDTO> Occurences { get; set; } = new List<OccurenceDTO>();

} 


