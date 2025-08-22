using MakerSchedule.Domain.ValueObjects;

namespace MakerSchedule.Domain.Entities;

public class EventTag
{
    public Guid Id { get; set; }
    public required EventTagName Name { get; set; } 
    public required string Color { get; set; } = string.Empty;
}