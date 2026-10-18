namespace TrafficVillage.Model;

public sealed record Vehicle(
    int Id,
    IReadOnlyList<EdgeId> Route,
    int CurrentEdgeIndex,
    double PositionOnEdge,
    double WaitTimeSeconds,
    VehicleState State);
