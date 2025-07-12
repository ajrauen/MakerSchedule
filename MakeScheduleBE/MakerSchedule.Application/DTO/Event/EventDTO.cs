using System.ComponentModel.DataAnnotations;
using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Application.DTO.Event;

public class EventDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public required string EventName { get; set; }

    [Required]
    public required string Description { get; set; }

    public EventTypeEnum EventType { get; set; }

    public int Duration { get; set; }
    public string? FileUrl { get; set; }
}