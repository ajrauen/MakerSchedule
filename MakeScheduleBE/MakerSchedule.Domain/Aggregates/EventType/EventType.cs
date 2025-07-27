
using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Domain.Aggregates.EventType;
public class EventType
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public required EventTypeName Name { get; set; }  
}