using MakerSchedule.Domain.Exceptions;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MakerSchedule.Domain.ValueObjects;

public record ScheduleStart
{
    public DateTime Value { get; }


    private ScheduleStart(DateTime date)
    {
        Value = date;
    }

    public static ScheduleStart Create(DateTime date)
    {
        if (!IsValid(date))
        {
            throw new ScheduleDateOutOfBoundsException("Schedule start must be in the future");
        }
        return new ScheduleStart(date);
    }

    private static bool IsValid(DateTime date)
    {
        return date >= DateTime.UtcNow;
    }


   public static ScheduleStart ForSeeding(DateTime date) => new ScheduleStart(date);

public static ValueConverter<ScheduleStart?, DateTime?> Converter =>
    new ValueConverter<ScheduleStart?, DateTime?>(
        scheduleStart => scheduleStart == null ? (DateTime?)null : scheduleStart.Value,
        value => value.HasValue ? ScheduleStart.ForSeeding(value.Value) : null);

    

    public static implicit operator DateTime?(ScheduleStart? duration) => duration?.Value;
    public static implicit operator ScheduleStart(DateTime seconds) => seconds;

}