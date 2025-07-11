namespace MakerSchedule.Application.Exceptions;
public class NotFoundException(string entityName, object id)
    : BaseException(
        message: $"{entityName} with id {id} was not found",
        errorCode: "NOT_FOUND",
        statusCode: 404
    )
{
}

