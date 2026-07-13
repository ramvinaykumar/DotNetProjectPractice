using BBS.Application.Commands;
using BBS.Application.Interfaces.Infrastructure;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Repositories.Reports;
using BBS.Application.Interfaces.Services;
using BBS.Application.Services;
using BBS.Infrastructure.ConnectionFactory;
using BBS.Infrastructure.Repositories;
using BBS.Infrastructure.Repositories.Reports;
using MediatR;

namespace BBS.API.Extensions
{
    /// <summary>
    /// Provides extension methods for registering application services and repositories with the dependency injection
    /// container.
    /// </summary>
    /// <remarks>Includes methods to configure custom services, repositories, and infrastructure dependencies
    /// for the application.</remarks>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers application-specific services, repositories, and factories for dependency injection.
        /// </summary>
        /// <param name="services">The service collection to add the custom services to.</param>
        /// <returns>The service collection with the custom services registered.</returns>
        public static IServiceCollection AddCustomDIServices(this IServiceCollection services)
        {
            // Register any custom services here
            // Example: services.AddScoped<IYourService, YourService>();

            // Register MediatR for handling commands and queries
            services.AddMediatR(typeof(CreatePassengerCommand).Assembly);

            // Register the connection factory
            services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBusRepository, BusRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();
            services.AddScoped<IRouteSeatAvailabilityRepository, RouteSeatAvailabilityRepository>();

            // Register services
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBusService, BusService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IRouteSeatAvailabilityService, RouteSeatAvailabilityService>();

            return services;
        }
    }
}
