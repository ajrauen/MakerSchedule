using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace MakerSchedule.Domain.ValueObjects;

public record EventTagName
{
    public string Value { get; }
    public EventTagName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Event tag name cannot be empty.", nameof(value));
        Value = value.Trim();
    }

    public static ValueConverter<EventTagName?, string?> Converter =>
      new ValueConverter<EventTagName?, string?>(
          eventTag => eventTag == null ? (string?)null : eventTag.Value,
          value => value != null ? new EventTagName(value) : null);

    public static implicit operator string?(EventTagName? eventTagName) => eventTagName?.Value;
    public static implicit operator EventTagName(string eventTagName) => new EventTagName(eventTagName);

}