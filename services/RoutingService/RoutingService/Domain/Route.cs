namespace RoutingService.Domain;

public record Route
{
    public double DistanceMeters { get; init; }
    public double DurationSeconds { get; init; }
    public IReadOnlyList<Waypoint> Waypoints { get; init; } = [];
    public string? Geometry { get; init; }
}