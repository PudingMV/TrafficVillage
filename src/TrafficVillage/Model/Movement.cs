namespace TrafficVillage.Model;

/// <summary>
/// One legal path through an intersection. Each movement has its own approach queue.
/// </summary>
public sealed record Movement(
    MovementId Id,
    NodeId IntersectionId,
    EdgeId Incoming,
    EdgeId Outgoing,
    NodeId FromNode,
    NodeId ToNode,
    TurnKind Turn);
