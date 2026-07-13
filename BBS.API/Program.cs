using BBS.API.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Core Framework - Controllers, Middleware, etc.
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(
        option=> option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

// Authentication & Authorization - JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Validation - FluentValidation
builder.Services.AddCustomFluentValidation(builder.Configuration);

// Swagger
builder.Services.AddSwaggerDocumentation();

// Dependency Injection Registrations
builder.Services.AddCustomDIServices();

// Telemetry & Monitoring - Application Insights (optional, can be configured later)
// builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the middleware pipeline
app.ConfigureMiddlewarePipeline();

app.Run();
