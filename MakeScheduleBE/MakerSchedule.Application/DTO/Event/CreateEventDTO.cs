using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace MakerSchedule.Application.DTO.Event;

public class CreateEventDTO
{
    [Required]
    public required string EventName { get; set; }

    [Required]
    public required string Description { get; set; }
    public int Duration { get; set; }
    [Required]
    public required Guid EventTypeId { get; set; }
    
    public required IFormFile FormFile  { get; set; }
}