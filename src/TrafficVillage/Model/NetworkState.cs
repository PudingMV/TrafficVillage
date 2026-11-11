namespace TrafficVillage.Model;

public sealed record NetworkState(
    double CurrentTimeSeconds,
    IReadOnlyList<IVehicle> Vehicles,
    IReadOnlyDictionary<NodeId, IntersectionState> Intersections);
