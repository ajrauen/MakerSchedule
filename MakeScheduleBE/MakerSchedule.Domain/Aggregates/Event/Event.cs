namespace MakerSchedule.Domain.Aggregates.Event;

using MakerSchedule.Domain.Enums;
using System.Collections.ObjectModel;
using MakerSchedule.Domain.Aggregates.Event;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Aggregate root for the Event aggregate.
/// </summary>
public class Event
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string EventName { get; set; }
    [Required]
    public required string Description { get; set; }
    public EventTypeEnum EventType { get; set; }
    public int Duration { get; set; }
    private readonly List<Occurrence> _occurrences = new();
    public IReadOnlyCollection<Occurrence> Occurrences => _occurrences.AsReadOnly();
    public string? FileUrl { get; set; }

    public Occurrence AddOccurrence(OccurrenceInfo info)
    {
        var occurrence = new Occurrence(this.Id, info);
        _occurrences.Add(occurrence);
        return occurrence;
    }

    // Remove an occurrence by ID
    public void RemoveOccurrence(int occurrenceId)
    {
        var occ = _occurrences.Find(o => o.Id == occurrenceId);
        if (occ != null)
            _occurrences.Remove(occ);
    }
} 