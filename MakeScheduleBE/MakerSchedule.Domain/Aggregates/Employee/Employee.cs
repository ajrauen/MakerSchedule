namespace MakerSchedule.Domain.Aggregates.Employee;


using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.Aggregates.Event;

public class Employee
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; } = string.Empty;  
    public User User { get; set; } = null!;  

    public string EmployeeNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }

    // Many-to-many navigation property
    public ICollection<Event> EventsLed { get; set; } = new List<Event>();
}
