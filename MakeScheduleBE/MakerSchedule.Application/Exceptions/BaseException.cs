using System;

namespace MakerSchedule.Application.Exceptions;

public class BaseException : Exception
{
    public string ErrorCode { get; }
    public int StatusCode { get; }

    public BaseException(string message, string errorCode = "GENERAL_ERROR", int statusCode = 500, Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}
