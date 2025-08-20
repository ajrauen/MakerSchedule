namespace MakerSchedule.Application.DTO.EventTag;
using System.ComponentModel.DataAnnotations;

public class CreateEventTagDTO
{
    [Required(ErrorMessage = "Event tag name is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 50 characters.")]
    public required string Name { get; set; } = string.Empty;

    public  string Color { get; set; } = string.Empty;
}
