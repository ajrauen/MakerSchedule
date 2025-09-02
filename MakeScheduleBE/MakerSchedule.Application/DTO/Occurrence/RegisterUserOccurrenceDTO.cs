namespace MakerSchedule.Application.DTO.Occurrence;

public class RegisterUserOccurrenceDTO
{
    public required Guid EventId { get; set; }
    public required Guid UserId { get; set; }
    public required Guid OccurrenceId { get; set; }
    public required int NumberOfSeats { get; set; }
}