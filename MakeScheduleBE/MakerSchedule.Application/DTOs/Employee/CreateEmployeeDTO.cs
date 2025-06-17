using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Application.DTOs.Employee
{
   public class CreateEmployeeDTOp
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string Phone { get; set; }

        [Required]
        [StringLength(200)]
        public required string Address { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}