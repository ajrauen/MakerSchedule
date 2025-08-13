namespace MakerSchedule.Domain.Aggregates.Event;

public class OccurrenceInfo(DateTime scheduleStart)
{
    public DateTime ScheduleStart { get; } = scheduleStart;

}