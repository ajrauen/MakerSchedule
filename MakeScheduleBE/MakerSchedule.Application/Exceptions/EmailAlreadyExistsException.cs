namespace MakerSchedule.Application.Exceptions;
public class EmailAlreadyExistsException(string email) 
    : BaseException($"Email address '{email}' is already registered", "EMAIL_ALREADY_EXISTS", 400)
{
}

