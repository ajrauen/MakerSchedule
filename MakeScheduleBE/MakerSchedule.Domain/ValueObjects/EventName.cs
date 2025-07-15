namespace MakerSchedule.Domain.ValueObjects;

public record EventName
{
    private string Value { get; init; }
    public EventName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), "Event Name cannot be empty");

        }

        Value = name.Trim();
    }

    public override string ToString() => Value;
}