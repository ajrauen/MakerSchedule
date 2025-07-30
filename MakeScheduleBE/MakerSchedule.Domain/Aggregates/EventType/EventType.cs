using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Domain.Aggregates.EventType;
using MakerSchedule.Domain.ValueObjects;
public class EventType
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public required EventTypeName Name { get; set; }  
}