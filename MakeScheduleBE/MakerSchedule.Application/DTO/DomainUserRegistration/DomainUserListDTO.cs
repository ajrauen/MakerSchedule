namespace MakerSchedule.Application.DTO.DomainUser;

public class DomainUserListDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required IEnumerable<string> Roles { get; set; }
    public required string Email { get; set; }
}
