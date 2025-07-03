namespace MakerSchedule.Application.DTOs.Event
{
    public class EventListDTO
    {
        public int Id { get; set; }
        public required string EventName { get; set; }
        public long ScheduleStart { get; set; }
        public int Duration { get; set; }
    }
}