using System.ComponentModel.DataAnnotations;

using MakerSchedule.Domain.Enums;

using Microsoft.AspNetCore.Http;

namespace MakerSchedule.Application.DTO.Event;

public class PatchEventDTO
{
    public string? EventName { get; set; }
    public string? Description { get; set; }
    public int? Duration { get; set; }
    public EventTypeEnum? EventType { get; set; }
    public IFormFile? FormFile { get; set; } 
}