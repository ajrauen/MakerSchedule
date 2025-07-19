namespace MakerSchedule.Application.DTO.DomainUser;

public class DomainUserListDTO
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
