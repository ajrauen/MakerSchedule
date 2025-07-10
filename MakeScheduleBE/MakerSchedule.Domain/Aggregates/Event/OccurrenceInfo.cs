namespace MakerSchedule.Domain.Aggregates.Event;

public class OccurrenceInfo
{
    public DateTime ScheduleStart { get; }
    public int? Duration { get; }
    public ICollection<int> Attendees { get; }
    public ICollection<int> Leaders { get; }

    public OccurrenceInfo(DateTime scheduleStart, int? duration, ICollection<int>? attendees, ICollection<int>? leaders)
    {
        ScheduleStart = scheduleStart;
        Duration = duration;
        Attendees = attendees ?? new List<int>();
        Leaders = leaders ?? new List<int>();
    }
} 