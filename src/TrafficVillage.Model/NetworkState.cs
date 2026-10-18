namespace TrafficVillage.Model;

public sealed record NetworkState(
    double CurrentTimeSeconds,
    IReadOnlyList<Vehicle> Vehicles,
    IReadOnlyDictionary<NodeId, IntersectionState> Intersections);
