using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Domain.Entities
{
    public class EventDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public required string EventName { get; set; }

        [Required]
        public required string Description { get; set; }

        public IEnumerable<Customer> Attendees { get; set; } = Array.Empty<Customer>();
        public IEnumerable<Employee> Leaders { get; set; } = Array.Empty<Employee>();

        public DateTime ScheduleStart { get; set; }
        
    }
}