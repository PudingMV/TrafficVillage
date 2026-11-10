namespace TrafficVillage.Model;

public sealed record Vehicle(
    int Id,
    NodeId Origin,
    NodeId Destination,
    IReadOnlyList<EdgeId> Route,
    int CurrentEdgeIndex,
    double PositionOnEdgeMeters,
    double WaitTimeSeconds,
    int StopCount,
    VehicleState State);
