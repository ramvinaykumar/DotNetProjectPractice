using BBS.Application.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BBS.API.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring authentication services.
    /// </summary>
    /// <remarks>Contains methods to set up JWT authentication and authorization in an ASP.NET Core
    /// application.</remarks>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Configures JWT bearer authentication and authorization using settings from the application configuration.
        /// </summary>
        /// <param name="services">The service collection to add authentication and authorization services to.</param>
        /// <param name="configuration">The configuration containing JWT settings such as issuer, audience, and secret key.</param>
        /// <returns>The service collection with JWT authentication and authorization configured.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the secret key is missing from the configuration.</exception>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("SecretKey missing")))
                    };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        await context.Response
                            .WriteAsJsonAsync(
                                new ApiResponse<object>
                                {
                                    Success = false,
                                    Message = "Authentication required. Please provide a valid access token.",
                                    Errors =
                                    [
                                        "AUTHENTICATION_FAILED"
                                    ]
                                });
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;

                        await context.Response
                            .WriteAsJsonAsync(
                                new ApiResponse<object>
                                {
                                    Success = false,
                                    Message = "You are not authorized to perform this action.",
                                    Errors =
                                    [
                                        "AUTHORIZATION_FAILED"
                                    ]
                                });
                    }
                };
            });

            services.AddAuthorization();

            return services;
        }
    }
}
