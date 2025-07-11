
namespace MakerSchedule.Application.Exceptions;

public class BaseException(
    string message,
    string errorCode = "GENERAL_ERROR",
    int statusCode = 500,
    Exception? innerException = null)
    : Exception(message, innerException)
{
    public string ErrorCode { get; } = errorCode;
    public int StatusCode { get; } = statusCode;
}