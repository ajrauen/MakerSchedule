namespace MakerSchedule.Application.DTO.DomainUser;

public class ResetPasswordWithTokenRequest
{
    public required Guid UserId { get; set; }
    public required string Token { get; set; }
    public required string NewPassword { get; set; }
}