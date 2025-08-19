using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.ValueObjects;

namespace MakerSchedule.Domain.Entities;

public class EventTag
{
    public Guid Id { get; set; }
    public required EventTagName Name { get; set; } = new EventTagName(string.Empty);
    public required string Category { get; set; } = string.Empty;
    public required string Color { get; set; } = string.Empty;
    public ICollection<Event> Events { get; set; } = new List<Event>();
}