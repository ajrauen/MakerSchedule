public record EventTypeName
{
    public string Value { get; }
    public EventTypeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Event type name cannot be empty.", nameof(value));
        Value = value.Trim();
    }

    
    public override string ToString() => Value;
}