using MakerSchedule.Application.DTOs.User;

namespace MakerSchedule.Application.DTOs.Customer
{
    public class UpdateCustomerProfileDTO : UpdateUserProfileDTO
    {
        // Customer-Specific Properties
        public string? PreferredContactMethod { get; set; }
        public string? Notes { get; set; }
    }
} 