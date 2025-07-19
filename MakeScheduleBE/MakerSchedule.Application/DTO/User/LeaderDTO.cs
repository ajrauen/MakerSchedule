namespace MakerSchedule.Application.DTO.User;

public class LeaderDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid Id { get; set; } =  Guid.NewGuid();
}