using BBS.Application.Common;
using BBS.Application.Validators.Booking;
using BBS.Application.Validators.Bus;
using BBS.Application.Validators.Passenger;
using BBS.Application.Validators.Route;
using BBS.Application.Validators.Schedule;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace BBS.API.Extensions
{
    /// <summary>
    /// Provides extension methods for registering FluentValidation services and custom validators in the application's
    /// dependency injection container.
    /// </summary>
    /// <remarks>Configures automatic validation, registers validators from specified assemblies, and
    /// customizes API behavior for model validation failures.</remarks>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Registers FluentValidation validators and configures API behavior for model validation errors.
        /// </summary>
        /// <param name="services">The service collection to add validators and configuration to.</param>
        /// <param name="configuration">The application configuration used for validation setup.</param>
        /// <returns>The service collection with FluentValidation and API behavior configured.</returns>
        public static IServiceCollection AddCustomFluentValidation(this IServiceCollection services, IConfiguration configuration)
        {
            // Register custom validators here if needed
            // Example: services.AddScoped<IValidator<YourModel>, YourModelValidator>();

            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssemblyContaining<CreateBookingValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateBusValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateRouteValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateScheduleValidator>();
            services.AddValidatorsFromAssemblyContaining<CreatePassengerValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdatePassengerValidator>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                                     .Where(x => x.Value?.Errors.Count > 0)
                                     .ToDictionary(
                                        x => char.ToLowerInvariant(x.Key[0]) + x.Key.Substring(1),
                                        x => x.Value!.Errors
                                            .Select(e => e.ErrorMessage)
                                            .ToArray()
                                     );

                    return new BadRequestObjectResult(
                        new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Validation Failed",
                            ValidationErrors = errors
                        });
                };
            });

            return services;
        }
    }
}
