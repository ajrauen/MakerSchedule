namespace MakerSchedule.Application.Exceptions
{
    public class EmailAlreadyExistsException : BaseException
    {
        public EmailAlreadyExistsException(string email) 
            : base($"Email address '{email}' is already registered", "EMAIL_ALREADY_EXISTS", 400)
        {
        }
    }
} 