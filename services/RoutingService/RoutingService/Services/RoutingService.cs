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

    public RouteService(IRoutingProvider routingProvider)
    {
        _routingProvider = routingProvider;
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

        return await _routingProvider.CalculateRouteAsync(
            locations,
            cancellationToken);
    }
}
