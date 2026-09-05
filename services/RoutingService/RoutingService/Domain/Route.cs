public class Route
{
    public double DistanceMeters { get; init; }
    public double DurationSeconds { get; init; }
    public IReadOnlyList<Waypoint> Legs { get; init; } = [];
}