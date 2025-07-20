using System.ComponentModel.DataAnnotations;

using MakerSchedule.Domain.Enums;

using Microsoft.AspNetCore.Http;

namespace MakerSchedule.Application.DTO.Event;

public class UpdateEventDTO
{
    [Required]
    public required Guid Id { get; set; } // or string if your PK is string
    [Required]
    public required string EventName { get; set; }
    [Required]
    public required string Description { get; set; }
    public int Duration { get; set; }
    [Required]
    public EventTypeEnum EventType { get; set; }
    public IFormFile? FormFile { get; set; } // Optional for update
}