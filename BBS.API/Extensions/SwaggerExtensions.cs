using Microsoft.OpenApi.Models;

namespace BBS.API.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring Swagger documentation and JWT Bearer authentication in an ASP.NET
    /// Core application.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Adds Swagger generation and configures JWT bearer authentication for API documentation.
        /// </summary>
        /// <param name="services">The service collection to add Swagger services to.</param>
        /// <returns>The modified service collection.</returns>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            return services;
        }
    }
}
