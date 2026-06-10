using HMS.Core.Interfaces;
using HMS.Infrastructure.Data;
using HMS.Infrastructure.Repositories;
using HMS.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace HMS.WebAPI.Extensions
{
    /// <summary>
    /// Extension methods that group related service registrations.
    /// Called from Program.cs to keep the startup file lean and readable.
    ///
    /// SERVICE LIFETIME QUICK REFERENCE
    /// ──────────────────────────────────────────────────────────────────────
    /// Singleton  → One instance for the entire app lifetime.
    ///              Use for: stateless, thread-safe objects (config readers,
    ///              connection factories that only hold a string, caches).
    ///
    /// Scoped     → One instance per HTTP request, disposed at request end.
    ///              Use for: repositories, services that wrap repositories,
    ///              anything that should live and die with one request.
    ///
    /// Transient  → New instance every time it is resolved.
    ///              Use for: lightweight stateless helpers (validators, mappers).
    ///              Avoid for: anything touching a DB connection.
    /// ──────────────────────────────────────────────────────────────────────
    /// </summary>
    public static class ServiceExtensions
    {
        // ── Infrastructure: DB factory + Repositories + Domain services ───────────
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services
            , IConfiguration config)
        {
            // ── Connection factory ─────────────────────────────────────────────────
            // LIFETIME: Singleton
            //
            // SqlConnectionFactory holds ONLY the connection-string (an immutable
            // string read once from IConfiguration).  It has no mutable state and
            // is fully thread-safe, so Singleton is correct.
            //
            // It does NOT open a SqlConnection in its constructor — it opens one
            // inside CreateConnection() each time a repository calls it.
            // ADO.NET's built-in connection pool manages the actual TCP sockets, so
            // opening/closing SqlConnection objects frequently is cheap and correct.
            //
            // Never register SqlConnection itself as Singleton — that would hold
            // one open connection for the whole app, causing concurrency failures.
            services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

            // ── Repositories ───────────────────────────────────────────────────────
            // LIFETIME: Scoped
            //
            // Why Scoped:
            //   • Each repository is injected by the DI container into a controller
            //     (which is also Scoped per request).
            //   • Scoped ensures the same repository instance is reused within one
            //     request if it is injected in multiple places — saving allocations.
            //   • Repositories open and dispose their SqlConnection inside each
            //     method with "using var conn = ...", so the repository object itself
            //     does not hold an open connection between method calls.
            //
            //  Singleton would be wrong — a Singleton repository is shared across
            //    all concurrent requests and could accidentally cache stale data.
            //  Transient would work functionally here, but wastes allocations
            //    because a new object would be created for every injection point
            //    within the same request.
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            // ── Application / Domain services ──────────────────────────────────────
            // LIFETIME: Scoped
            //
            // TokenService reads IConfiguration (Singleton-safe) and generates JWT
            // tokens — it holds no mutable state per call, so Transient would also
            // work.  Scoped is the idiomatic choice for service classes because it
            // future-proofs them: if a Scoped dependency (e.g. a current-user
            // context) is injected later, the lifetime is already correct.
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        // ── Caching ───────────────────────────────────────────────────────────────
        public static IServiceCollection AddCaching(this IServiceCollection services)
        {
            // Both are internally Singleton (managed by the ASP.NET Core framework).
            // IMemoryCache → one shared in-process cache for the app lifetime.
            // Response caching services → used by UseResponseCaching() middleware.
            services.AddMemoryCache();
            services.AddResponseCaching();
            return services;
        }

        // ── JWT Authentication ─────────────────────────────────────────────────────
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var jwtSettings = config.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // set true in production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero       // no tolerance for expired tokens
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                            context.Response.Headers.Append("Token-Expired", "true");
                        return Task.CompletedTask;
                    }
                };
            });

            // Registers IAuthorizationService (Singleton) and policy infrastructure.
            services.AddAuthorization();
            return services;
        }

        // ── Swagger / OpenAPI ──────────────────────────────────────────────────────
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hotel Management API",
                    Version = "v1",
                    Description = "Complete REST API for the Hotel Management System — " +
                              "Rooms, Bookings, Customers, Payments, Staff and Dashboard.",
                    Contact = new OpenApiContact { Name = "Hotel Dev Team" }
                });

                // JWT support in Swagger UI
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header. Enter: Bearer {your-token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                // Render enum values as string names in Swagger, not integers
                c.UseInlineDefinitionsForEnums();
            });

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration config)
        {
            var origins = config.GetSection("AllowedOrigins").Get<string[]>()
                      ?? Array.Empty<string>();

            services.AddCors(options =>
            {
                options.AddPolicy("HotelCorsPolicy", policy =>
                {
                    policy.WithOrigins(origins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }
    }
}
