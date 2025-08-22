
namespace MakerSchedule.Application.DTO.Event;

public class EventListDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string EventName { get; set; }
    public string Description { get; set; } = String.Empty;
    public int? Duration { get; set; }
    public string? ThumbnailUrl { get; set; }
    
    public required Guid[] EventTagIds { get; set; } = Array.Empty<Guid>();
}