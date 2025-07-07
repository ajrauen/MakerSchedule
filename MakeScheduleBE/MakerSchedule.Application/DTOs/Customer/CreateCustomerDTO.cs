namespace MakerSchedule.Application.DTOs.Customer;

public class CreateCustomerDTO
{
    // User fields (required for account creation)
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    
    // Customer-specific fields
    public string? PreferredContactMethod { get; set; }
    public string? Notes { get; set; }
} 