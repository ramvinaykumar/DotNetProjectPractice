using PizzaFactoryApi.Factories;
using PizzaFactoryApi.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Scrutor NuGet 
builder.Services.Scan(scan => scan
    .FromAssemblyOf<IPizza>()
    .AddClasses(classes => classes.AssignableTo<IPizza>())
    .AsImplementedInterfaces()
    .WithTransientLifetime());

builder.Services.AddSingleton<PizzaFactory>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
