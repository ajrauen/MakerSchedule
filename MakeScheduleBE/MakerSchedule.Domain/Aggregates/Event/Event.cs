namespace MakerSchedule.Domain.Aggregates.Event;


using System.ComponentModel.DataAnnotations;

using MakerSchedule.Domain.Entities;
using MakerSchedule.Domain.ValueObjects;

/// <summary>
/// Aggregate root for the Event aggregate.
/// </summary>
public class Event
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public EventName EventName { get; set; } = null!;
    [Required]
    public required string Description { get; set; }

    public Duration? Duration { get; set; }
    private readonly List<Occurrence> _occurrences = new();
    public IReadOnlyCollection<Occurrence> Occurrences => _occurrences.AsReadOnly();

    private readonly List<EventTag> _eventTags = new();
    public IReadOnlyCollection<EventTag> EventTags => _eventTags.AsReadOnly();
    public string? ThumbnailUrl { get; set; }

    public Occurrence AddOccurrence(OccurrenceInfo info)
    {
        if (info == null) throw new ArgumentNullException(nameof(info));

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

    public void AddEventTag(EventTag tag)
    {
        if (tag == null) throw new ArgumentNullException(nameof(tag));
        _eventTags.Add(tag);
    }
    
    public void RemoveEventTag(Guid tagId)
    {
        var tag = _eventTags.Find(t => t.Id == tagId);
        if (tag != null)
            _eventTags.Remove(tag);
    }

} 