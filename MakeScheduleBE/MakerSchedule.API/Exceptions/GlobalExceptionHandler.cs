using System.Net;
using System.Text.Json;

using MakerSchedule.Application.Exceptions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.API.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
{

    

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception has occurred");

        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, errorCode, message) = exception switch
        {
            BaseException baseException => (baseException.StatusCode, baseException.ErrorCode, baseException.Message),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "NOT_FOUND", exception.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "UNAUTHORIZED", exception.Message),
            NullReferenceException => (StatusCodes.Status404NotFound, "NOT_FOUND", exception.Message),
            ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, "BAD_REQUEST", exception.Message),
            ArgumentNullException => (StatusCodes.Status400BadRequest, "BAD_REQUEST", exception.Message),
            ArgumentException => (StatusCodes.Status400BadRequest, "BAD_REQUEST", exception.Message),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "VALIDATION_ERROR", exception.Message),
            Microsoft.IdentityModel.Tokens.SecurityTokenException => (StatusCodes.Status401Unauthorized, "INVALID_TOKEN", "Invalid or expired token"),
            _ => (StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR", "An unexpected error occurred")
        };

        response.StatusCode = statusCode;

        var result = JsonSerializer.Serialize(new
        {
            error = message,
            code = errorCode,
            timestamp = DateTime.UtcNow
        });

        await response.WriteAsync(result, cancellationToken);
        return true;
    }
}
