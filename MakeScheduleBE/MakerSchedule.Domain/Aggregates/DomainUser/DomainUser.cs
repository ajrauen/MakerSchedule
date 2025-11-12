namespace MakerSchedule.Domain.Aggregates.DomainUser;

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.ValueObjects;


public class DomainUser
{
    [Key]
    public Guid Id { get; set; } =  Guid.NewGuid();
    [Required]
    public Guid UserId { get; set; } =  Guid.NewGuid();
    public User User { get; set; } = null!;
    public string PreferredContactMethod { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public required Email Email { get; set; }
    public required PhoneNumber PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties for many-to-many relationships
    public ICollection<OccurrenceLeader> OccurrencesLed { get; set; } = new List<OccurrenceLeader>();
    public ICollection<OccurrenceAttendee> OccurrenceRegistrations { get; set; } = new List<OccurrenceAttendee>();
}
