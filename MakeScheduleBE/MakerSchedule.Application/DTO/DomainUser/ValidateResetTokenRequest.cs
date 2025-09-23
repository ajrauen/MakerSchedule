namespace MakerSchedule.Application.DTO.DomainUser;
public class ValidateResetTokenRequest
{
    public required string Token { get; set; }
    public required Guid UserId { get; set; }
}