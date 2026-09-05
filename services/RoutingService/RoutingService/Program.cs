using RoutingService.Providers;
using RoutingService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<IRoutingProvider, OsrmRoutingProvider>(client =>
{
    client.BaseAddress = new Uri("https://router.project-osrm.org");
});

builder.Services.AddScoped<IRouteService, RouteService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
