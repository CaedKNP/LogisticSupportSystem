using System.Globalization;
using System.Net.Http.Json;
using RoutingService.Domain;
using Route = RoutingService.Domain.Route;

namespace RoutingService.Providers;

public interface IRoutingProvider
{
    Task<Route> CalculateRouteAsync(
        IReadOnlyList<Location> locations,
        CancellationToken cancellationToken = default);
}

public class OsrmRoutingProvider : IRoutingProvider
{
    private readonly HttpClient _httpClient;

    public OsrmRoutingProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Route> CalculateRouteAsync(
        IReadOnlyList<Location> locations,
        CancellationToken cancellationToken = default)
    {
        var coordinates = string.Join(
            ";",
            locations.Select(x =>
                $"{x.Longitude.ToString(CultureInfo.InvariantCulture)}," +
                $"{x.Latitude.ToString(CultureInfo.InvariantCulture)}")
        );

        var response = await _httpClient.GetFromJsonAsync<OsrmResponse>(
            $"/route/v1/driving/{coordinates}?overview=full&geometries=polyline",
            cancellationToken
        );

        if (response is null || response.Routes.Count == 0)
        {
            throw new InvalidOperationException(
                "OSRM could not calculate a route.");
        }

        var route = response.Routes[0];

        return new Route
        {
            DistanceMeters = route.Distance,
            DurationSeconds = route.Duration,

            Waypoints = locations
                .Select((location, index) =>
                    new Waypoint(index.ToString(), location))
                .ToList(),

            Geometry = route.Geometry
        };
    }

    private sealed class OsrmResponse
    {
        public List<OsrmRoute> Routes { get; set; } = [];
    }

    private sealed class OsrmRoute
    {
        public double Distance { get; set; }
        public double Duration { get; set; }
        public string Geometry { get; set; } = "";
    }
}