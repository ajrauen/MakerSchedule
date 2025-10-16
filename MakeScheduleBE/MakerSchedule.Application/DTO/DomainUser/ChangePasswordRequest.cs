namespace MakerSchedule.Application.DTO.DomainUser;

public class ChangePasswordRequest
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}