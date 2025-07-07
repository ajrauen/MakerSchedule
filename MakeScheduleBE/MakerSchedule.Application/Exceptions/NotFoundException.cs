using System;

namespace MakerSchedule.Application.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string entityName, object id)
        : base(
            message: $"{entityName} with id {id} was not found",
            errorCode: "NOT_FOUND",
            statusCode: 404
        )
    {
    }
}
