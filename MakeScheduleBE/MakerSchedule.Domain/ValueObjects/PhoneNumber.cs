using System;
using System.Text.RegularExpressions;

namespace MakerSchedule.Domain.ValueObjects;

public sealed record PhoneNumber
{
    public string Value { get; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone number cannot be empty.", nameof(value));
        // Basic phone regex for demonstration (US-centric)
        if (!Regex.IsMatch(value, @"^\+?[0-9]{7,15}$"))
            throw new ArgumentException("Invalid phone number format.", nameof(value));
        Value = value;
    }

    public override string ToString() => Value;
}
