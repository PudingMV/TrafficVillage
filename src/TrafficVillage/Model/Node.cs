namespace TrafficVillage.Model;

public sealed record Node(
    NodeId Id,
    Vec2 Position,
    NodeKind Kind,
    bool HasTrafficLight);
