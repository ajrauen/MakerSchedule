namespace MakerSchedule.Domain.Aggregates.Event;

public class OccurrenceInfo(DateTime scheduleStart, int? duration)
{
    public DateTime ScheduleStart { get; } = scheduleStart;
    public int? Duration { get; } = duration;

}