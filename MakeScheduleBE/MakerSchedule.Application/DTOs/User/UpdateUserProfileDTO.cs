namespace MakerSchedule.Application.DTOs.User
{
    public class UpdateUserProfileDTO
    {
        // User Profile Properties
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
    }
} 