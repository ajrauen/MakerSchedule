using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Application.DTOs.CustomerRegistration;

public class CustomerRegistrationDTO
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    public required string PhoneNumber { get; set; }

    [Required]
    public required string Address { get; set; }

    public required string CustomerNumber { get; set; }
    public required string PreferredContactMethod { get; set; }
    public required string Notes { get; set; }
}
