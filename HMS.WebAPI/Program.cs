using HMS.WebAPI.Extensions;
using HMS.WebAPI.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ── 1. Logging (Serilog) ─────────────────────────────────────────────────────
// Must be configured before anything else so that startup errors are captured.
// Serilog registers its ILogger as a Singleton via builder.Host.UseSerilog().
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  // reads from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/hms-api-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ── 2. Controllers + JSON serialization ──────────────────────────────────────
// AddControllers registers:
//   IControllerFactory          (Singleton)
//   IActionDescriptorCollection (Singleton)
//   Controller instances        (Scoped — one per request, resolved from DI)
//   Model binders, filters, etc.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Return camelCase JSON to match Angular / JS conventions
        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;

        // Omit null fields from responses to keep payloads clean
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

        // Write enum values as their string names, not integers
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddInfrastructure(builder.Configuration);
//builder.Services.AddJwtAuthentication(builder.Configuration);
//builder.Services.AddCorsPolicy(builder.Configuration);
//builder.Services.AddSwaggerGen();
//builder.Services.AddHttpContextAccessor();

// ── 3. HttpContext accessor ───────────────────────────────────────────────────
// LIFETIME: Singleton wrapper, but it exposes the per-request HttpContext.
// Required if any service needs to read the current user / request headers
// outside of a controller action (e.g. audit logging, current-user service).
builder.Services.AddHttpContextAccessor();

// ── 4. Infrastructure: DB factory + Repositories + TokenService ───────────────
// Registers:
//   SqlConnectionFactory      → Singleton
//   All six repositories      → Scoped
//   TokenService              → Scoped
// (See ServiceExtensions.AddInfrastructure for full lifetime explanations)
builder.Services.AddInfrastructure(builder.Configuration);
// ── 5. Caching ────────────────────────────────────────────────────────────────
// Registers:
//   IMemoryCache              → Singleton
//   Response caching services → Singleton (used by UseResponseCaching middleware)
builder.Services.AddCaching();

// ── 6. JWT Authentication + Authorization ─────────────────────────────────────
// Registers:
//   JwtBearerHandler          → Singleton (stateless token validator)
//   IAuthorizationService     → Singleton
//   Authorization policies    → Singleton
builder.Services.AddJwtAuthentication(builder.Configuration);

// ── 7. CORS ───────────────────────────────────────────────────────────────────
// Registers CORS options and policy (Singleton).
// Allowed origins are read from appsettings.json → "AllowedOrigins".
builder.Services.AddCorsPolicy(builder.Configuration);

// ── 8. Swagger / OpenAPI ──────────────────────────────────────────────────────
// Registers Swagger generator services (Singleton).
// UI is only enabled in Development (see pipeline below).
builder.Services.AddSwagger();

// ═════════════════════════════════════════════════════════════════════════════
//  BUILD & CONFIGURE THE HTTP PIPELINE
// ═════════════════════════════════════════════════════════════════════════════

var app = builder.Build();

// ── Global exception handler (must be FIRST in the pipeline) ─────────────────
// Catches all unhandled exceptions and maps them to JSON ApiResponse<T> with
// the appropriate HTTP status code. Prevents stack traces leaking to clients.
app.UseMiddleware<ExceptionMiddleware>();

// ── Swagger UI (Development only) ────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Management API v1");
        c.RoutePrefix = "swagger";  // Serve Swagger at http://localhost:5000/
        c.DisplayRequestDuration();
    });
}

// ── Serilog request logging ───────────────────────────────────────────────────
// Logs each HTTP request with method, path, status code and elapsed time.
app.UseSerilogRequestLogging();

// ── HTTPS redirection ─────────────────────────────────────────────────────────
// Redirects HTTP → HTTPS in production. Keep this before authentication.
app.UseHttpsRedirection();

// ── Response caching middleware ───────────────────────────────────────────────
// Must come before routing. Serves cached responses for endpoints decorated
// with [ResponseCache]. Registered via AddCaching() above.
app.UseResponseCaching();

// ── CORS ──────────────────────────────────────────────────────────────────────
// Must come BEFORE UseAuthentication and UseAuthorization.
// Must come AFTER UseRouting (implicitly added by MapControllers).
app.UseCors("HotelCorsPolicy");

// ── Authentication → Authorization ───────────────────────────────────────────
// ORDER MATTERS: Authentication must always precede Authorization.
// UseAuthentication reads the JWT from the request header and populates
// HttpContext.User.  UseAuthorization then evaluates [Authorize] attributes.
app.UseAuthentication();
app.UseAuthorization();

// ── Controllers ───────────────────────────────────────────────────────────────
app.MapControllers();

// ── Start ─────────────────────────────────────────────────────────────────────
Log.Information(
    "Hotel Management API starting | Environment: {Env} | .NET {Version}",
    app.Environment.EnvironmentName,
    System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);

app.Run();
