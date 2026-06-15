namespace BBS.API.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring the application's middleware pipeline.
    /// </summary>
    /// <remarks>Includes methods to set up Swagger for API documentation, enforce HTTPS redirection, handle
    /// global exceptions, and configure authentication and authorization.</remarks>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures the middleware pipeline with Swagger, HTTPS redirection, global exception handling,
        /// authentication, authorization, and controller endpoint mapping.
        /// </summary>
        /// <param name="app">The web application to configure.</param>
        /// <returns>The configured web application.</returns>
        public static WebApplication ConfigureMiddlewarePipeline(this WebApplication app)
        {
            // Add any custom middleware here if needed
            // Example: app.UseMiddleware<YourCustomMiddleware>();

            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Security - HTTPS Redirection
            app.UseHttpsRedirection();

            // Global exception handling should be early
            app.UseGlobalExceptionMiddleware();

            // Security - CORS, Authentication, Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Endpoints
            app.MapControllers();

            return app;
        }
    }
}
