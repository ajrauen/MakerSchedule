namespace MakerSchedule.Application.DTO.User;

public class UpdateUserProfileDTO 
{
    public Guid UserId { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? Role { get; set; } = Domain.Constants.Roles.Customer;
     public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public bool? IsActive { get; set; }
}