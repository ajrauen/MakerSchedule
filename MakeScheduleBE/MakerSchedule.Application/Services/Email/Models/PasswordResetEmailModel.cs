public class PasswordResetEmailModel
{
    public required string UserName { get; set; }
    public required string ResetLink { get; set; }
}