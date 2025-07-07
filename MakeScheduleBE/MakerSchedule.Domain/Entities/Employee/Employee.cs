namespace MakerSchedule.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Employee
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; } = string.Empty;  // Foreign key to User
    public User User { get; set; } = null!;  // Navigation property

    // Additional Employee-specific properties
    public string EmployeeNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }

    // Many-to-many navigation property
    public ICollection<Event> EventsLed { get; set; } = new List<Event>();
}
