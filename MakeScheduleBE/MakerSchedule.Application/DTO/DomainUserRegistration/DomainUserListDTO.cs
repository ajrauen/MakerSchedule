namespace MakerSchedule.Application.DTO.DomainUser;

public class DomainUserListDTO
{
    public string Id { get; set; } = string.Empty;
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
