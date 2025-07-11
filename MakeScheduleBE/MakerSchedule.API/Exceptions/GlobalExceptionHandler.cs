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
            InvalidOperationException => (StatusCodes.Status400BadRequest, "VALIDATION_ERROR", exception.Message),
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
