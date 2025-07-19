using MakerSchedule.Domain.Exceptions;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MakerSchedule.Domain.ValueObjects;

public record ScheduleStart
{
    public DateTime Value { get;  }

    public ScheduleStart(DateTime date)
    {
        if (date <= DateTime.UtcNow)
        {
            throw new ScheduleDateOutOfBoundsException("Schedule start must be in the future");
        }
        Value = date;
    }

    public static ScheduleStart ForSeeding(DateTime date) => new ScheduleStart(date, skipCheck: true);

    private ScheduleStart(DateTime date, bool skipCheck)
    {
        Value = date;
    }


    public static ValueConverter<ScheduleStart?, DateTime?> Converter =>
        new ValueConverter<ScheduleStart?, DateTime?>(
            scheduleStart => scheduleStart == null ? (DateTime?)null : scheduleStart.Value,
            value => value.HasValue ? new ScheduleStart(value.Value) : null);

    

    public static implicit operator DateTime?(ScheduleStart? duration) => duration?.Value;
    public static implicit operator ScheduleStart(DateTime seconds) => seconds;

}