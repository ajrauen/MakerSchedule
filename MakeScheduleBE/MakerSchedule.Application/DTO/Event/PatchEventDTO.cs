using Microsoft.AspNetCore.Http;

namespace MakerSchedule.Application.DTO.Event;

public class PatchEventDTO
{
    public string? EventName { get; set; }
    public string? Description { get; set; }
    public int? Duration { get; set; }
    public Guid[]? EventTagIds { get; set; }
    public IFormFile? FormFile { get; set; } 
    public int? ClassSize { get; set; }
    public decimal? Price { get; set; }

}