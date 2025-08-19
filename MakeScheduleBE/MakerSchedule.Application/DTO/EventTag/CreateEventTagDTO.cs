namespace MakerSchedule.Application.DTO.EventTag;
using System.ComponentModel.DataAnnotations;

public class CreateEventTagDTO
{
    [Required(ErrorMessage = "Event tag name is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 50 characters.")]
    public required string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Event tag color is required.")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Color must be a valid hex color code.")]
    public required string Color { get; set; } = string.Empty;
}
