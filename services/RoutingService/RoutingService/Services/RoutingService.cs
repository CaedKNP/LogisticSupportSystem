using RoutingService.Domain;
using RoutingService.Providers;
using Route = RoutingService.Domain.Route;

namespace RoutingService.Services;

public interface IRouteService
{
    Task<Route> CalculateRouteAsync(
        IReadOnlyList<Location> locations,
        CancellationToken cancellationToken = default);
}

public sealed class RouteService : IRouteService
{
    private readonly IRoutingProvider _routingProvider;
    private readonly ILogger<RouteService> _logger;

    public RouteService(IRoutingProvider routingProvider, ILogger<RouteService> logger)
    {
        _routingProvider = routingProvider;
        _logger = logger;
    }

    public async Task<Route> CalculateRouteAsync(
        IReadOnlyList<Location> locations,
        CancellationToken cancellationToken = default)
    {
        if (locations is null)
        {
            throw new ArgumentNullException(nameof(locations));
        }

        if (locations.Count < 2)
        {
            throw new ArgumentException(
                "At least two locations are required to calculate a route.",
                nameof(locations));
        }

        _logger.LogInformation( "Calculating route for {LocationCount} locations", locations.Count); 

        var route = await _routingProvider.CalculateRouteAsync( locations, cancellationToken);

        _logger.LogInformation( "Route calculated successfully. Distance: {DistanceMeters} m, Duration: {DurationSeconds} seconds", 
            route.DistanceMeters, 
            route.DurationSeconds); 
        
        return route;
    }
}
