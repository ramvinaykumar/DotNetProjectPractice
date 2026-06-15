using BBS.API.Middleware;

namespace BBS.API.Extensions
{
    /// <summary>
    /// Provides extension methods for adding middleware components to the application's request pipeline.
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Global exception middleware extension method for IApplicationBuilder to handle exceptions in the request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder app</param>
        /// <returns></returns>
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
