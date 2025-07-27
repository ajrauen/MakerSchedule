namespace MakerSchedule.Domain.Aggregates.Event;


using System.ComponentModel.DataAnnotations;

using MakerSchedule.Domain.Aggregates.EventType;
using MakerSchedule.Domain.ValueObjects;

/// <summary>
/// Aggregate root for the Event aggregate.
/// </summary>
public class Event
{
    [Key]
    public Guid Id { get; set; } =  Guid.NewGuid();
    public EventName EventName { get; set; } = null!;
    [Required]
    public required string Description { get; set; }

    public required Guid EventTypeId { get; set; }
    public  EventType? EventType { get; set; }
    public Duration? Duration { get; set; }
    private readonly List<Occurrence> _occurrences = new();
    public IReadOnlyCollection<Occurrence> Occurrences => _occurrences.AsReadOnly();
    public string? ThumbnailUrl { get; set; }

    public Occurrence AddOccurrence(OccurrenceInfo info)
    {
        var occurrence = new Occurrence(this.Id, info);
        _occurrences.Add(occurrence);
        return occurrence;
    }

    public void RemoveOccurrence(Guid occurrenceId)
    {
        var occ = _occurrences.Find(o => o.Id == occurrenceId);
        if (occ != null)
            _occurrences.Remove(occ);
    }
} 