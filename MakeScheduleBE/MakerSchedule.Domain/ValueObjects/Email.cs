using System.Text.RegularExpressions;

namespace MakerSchedule.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.", nameof(value));
        // Basic email regex for demonstration
        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email format.", nameof(value));
        Value = value;
    }

    public override string ToString() => Value;
}
