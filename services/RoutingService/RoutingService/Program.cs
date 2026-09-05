using RoutingService.Providers;
using RoutingService.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<IRoutingProvider, OsrmRoutingProvider>(client =>
{
    client.BaseAddress = new Uri("http://router.project-osrm.org");
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "LogisticSupportSystem-RoutingService/1.0");
});

builder.Host.UseSerilog((context, configuration) => 
{ 
    configuration .MinimumLevel.Information() .WriteTo.Console(); 
});

builder.Services.AddScoped<IRouteService, RouteService>();

// controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.MapControllers();


app.Run();
