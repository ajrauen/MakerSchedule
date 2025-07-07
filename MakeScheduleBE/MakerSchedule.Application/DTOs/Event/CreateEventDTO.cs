using System.ComponentModel.DataAnnotations;

using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Domain.Entities;

public class CreateEventDTO
{
    [Required]
    public required string EventName { get; set; }

    [Required]
    public required string Description { get; set; }
    public int Duration { get; set; }
    [Required]
    public EventTypeEnum EventType{ get; set; }
}