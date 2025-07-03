using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MakerSchedule.Domain.Entities
{
    public class Event
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public required string EventName { get; set; }

        [Required]
        public required string Description { get; set; }

        // Navigation properties for many-to-many
        public ICollection<Customer> Attendees { get; set; } = new List<Customer>();
        public ICollection<Employee> Leaders { get; set; } = new List<Employee>();

        public DateTime ScheduleStart { get; set; }
        public int Duration { get; set; }
    }
}