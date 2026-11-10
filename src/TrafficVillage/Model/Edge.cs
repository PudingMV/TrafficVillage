namespace TrafficVillage.Model;

/// <summary>
/// Directed road segment. A two-way street is two edges in opposite directions.
/// </summary>
public sealed record Edge(
    EdgeId Id,
    NodeId From,
    NodeId To,
    double LengthMeters,
    double SpeedMetersPerSecond);
