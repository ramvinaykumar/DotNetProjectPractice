using BBS.Application.Common;
using FluentValidation;
using System.Net;

namespace BBS.API.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions during HTTP request processing.
    /// </summary>
    /// <remarks>Logs exceptions and returns standardized HTTP responses based on exception type.</remarks>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the ExceptionMiddleware class for handling exceptions in the request pipeline.
        /// </summary>
        /// <param name="next">The next middleware delegate in the HTTP request pipeline.</param>
        /// <param name="logger">The logger used to record exception details.</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Processes HTTP requests and handles validation, business, and unhandled exceptions by returning appropriate
        /// error responses.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                await context.Response.WriteAsJsonAsync(
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation Failed",
                        Errors = ex.Errors.Select(x => x.ErrorMessage).ToList()
                    });
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                await context.Response.WriteAsJsonAsync(
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = ex.Message
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsJsonAsync(
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "An unexpected error occurred."
                    });
            }
        }
    }
}
