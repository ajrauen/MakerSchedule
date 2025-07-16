namespace MakerSchedule.Domain.Aggregates.DomainUser;

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.Aggregates.Event;

public class DomainUser
{
    [Key]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public string PreferredContactMethod { get; set; } = string.Empty;

    // Navigation properties for many-to-many relationships
    public ICollection<OccurrenceLeader> OccurrencesLed { get; set; } = new List<OccurrenceLeader>();
    public ICollection<OccurrenceAttendee> OccurrencesAttended { get; set; } = new List<OccurrenceAttendee>();
}
