using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MakerSchedule.Application.Exceptions;

namespace MakerSchedule.API.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

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
} 