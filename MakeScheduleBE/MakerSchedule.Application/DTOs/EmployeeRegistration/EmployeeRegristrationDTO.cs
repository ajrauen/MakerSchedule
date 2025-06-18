using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Application.DTOs.EmployeeRegistration;

public class EmployeeRegristrationDTO
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
} 