namespace MakerSchedule.Domain.Entities;

using Microsoft.AspNetCore.Identity;
using MakerSchedule.Domain.Enums;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser
{
    [Key]
    public override string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public UserType UserType { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navigation properties
    public Employee? Employee { get; set; }
    public Customer? Customer { get; set; }
}
