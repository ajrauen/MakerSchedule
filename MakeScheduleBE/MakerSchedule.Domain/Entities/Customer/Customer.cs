using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Domain.Entities;

public class Customer
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; } = string.Empty;  // Foreign key to User
    public User User { get; set; } = null!;  // Navigation property

    // Additional Customer-specific properties
    public string CustomerNumber { get; set; } = string.Empty;
    public string PreferredContactMethod { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
