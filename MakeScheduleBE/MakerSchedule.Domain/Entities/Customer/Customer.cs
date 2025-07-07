namespace MakerSchedule.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Customer
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; } = string.Empty;  // Foreign key to User
    public User User { get; set; } = null!;  // Navigation property

    // Additional Customer-specific properties
    public string CustomerNumber { get; set; } = string.Empty;
    public string PreferredContactMethod { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    // Many-to-many navigation property
    public ICollection<Event> EventsAttended { get; set; } = new List<Event>();
}
