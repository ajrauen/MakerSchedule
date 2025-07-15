using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MakerSchedule.Domain.ValueObjects;
public record Duration
{
    private int Value { get; init; }

    public Duration(int value)
    {
        if (value < 0 || value > 1440 * 60 * 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Duration must be between 0 and 24 hours in milliseconds");
        }
        Value = value;
    }

  public static ValueConverter<Duration?, int?> Converter =>
    new ValueConverter<Duration?, int?>(
        duration => duration == null ? (int?)null : duration.Value,
        value => value.HasValue ? new Duration(value.Value) : null);

    public static implicit operator int?(Duration? duration) => duration?.Value;
    public static implicit operator Duration(int seconds) => seconds;

}