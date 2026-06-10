using HMS.Core.Dtos.Response;
using System.Net;
using System.Text.Json;

namespace HMS.WebAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            // SQL Server error numbers for common business rule violations
            var isSqlError = ex.Message.Contains("RAISERROR", StringComparison.OrdinalIgnoreCase)
                             || ex.GetType().Name.Contains("SqlException");
            var isConflict = ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase);
            var isNotFound = ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase);
            var isBadRequest = ex.Message.Contains("not available", StringComparison.OrdinalIgnoreCase)
                             || ex.Message.Contains("cannot", StringComparison.OrdinalIgnoreCase)
                             || ex.Message.Contains("must be", StringComparison.OrdinalIgnoreCase);

            int statusCode = ex switch
            {
                _ when isConflict => (int)HttpStatusCode.Conflict,
                _ when isNotFound => (int)HttpStatusCode.NotFound,
                _ when isBadRequest => (int)HttpStatusCode.BadRequest,
                _ when isSqlError => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;

            // Don't expose raw internal errors in production
            var message = statusCode == (int)HttpStatusCode.InternalServerError
                ? "An unexpected error occurred. Please try again later."
                : ex.Message;

            var response = ApiResponse<object>.Fail(message);
            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
        }
    }
}
