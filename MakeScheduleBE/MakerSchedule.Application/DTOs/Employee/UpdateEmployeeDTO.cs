using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Application.DTOs.Employee
{
    public class UpdateEmployeeDTO
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public bool? IsActive { get; set; }
    }
} 