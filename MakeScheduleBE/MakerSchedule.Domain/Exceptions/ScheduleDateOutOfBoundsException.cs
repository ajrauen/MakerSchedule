namespace MakerSchedule.Domain.Exceptions;

public class ScheduleDateOutOfBoundsException(string message)
    : Exception(message)
{
}