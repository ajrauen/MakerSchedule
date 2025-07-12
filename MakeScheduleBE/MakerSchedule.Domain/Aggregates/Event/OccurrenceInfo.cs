public class OccurrenceInfo(DateTime scheduleStart, int? duration)
{
    public DateTime ScheduleStart { get; } = scheduleStart;
    public int? Duration { get; } = duration;
    // Note: Leaders and Attendees are now managed through join entities
    // and will be set up after the Occurrence is created
}