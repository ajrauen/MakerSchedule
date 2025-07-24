namespace MakerSchedule.Domain.Aggregates.User;


using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using MakerSchedule.Domain.Aggregates.DomainUser;

public class User : IdentityUser<Guid>
{
    [Key]
    public override Guid Id { get; set; } = Guid.Empty;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    // Navigation properties
    public DomainUser? DomainUser { get; set; }
}
