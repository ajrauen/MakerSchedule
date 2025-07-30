using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace MakerSchedule.Domain.ValueObjects;

public record EventTypeName
{
    public string Value { get; }
    public EventTypeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Event type name cannot be empty.", nameof(value));
        Value = value.Trim();
    }

    public static ValueConverter<EventTypeName?, string?> Converter =>
      new ValueConverter<EventTypeName?, string?>(
          eventType => eventType == null ? (string?)null : eventType.Value,
          value => value != null ? new EventTypeName(value) : null);

    public static implicit operator string?(EventTypeName? eventTypeName) => eventTypeName?.Value;
    public static implicit operator EventTypeName(string eventTypeName) => new EventTypeName(eventTypeName);

}