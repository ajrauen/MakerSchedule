using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Application.DTOs.CustomerRegistration;

public class CustomerRegistrationDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string Address { get; set; }

    public string CustomerNumber { get; set; }
    public string PreferredContactMethod { get; set; }
    public string Notes { get; set; }
}
