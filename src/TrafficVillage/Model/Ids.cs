namespace TrafficVillage.Model;

public readonly record struct NodeId(string Value)
{
    public override string ToString() => Value;
}

public readonly record struct PhaseId(string Value)
{
    public override string ToString() => Value;
}

public readonly record struct MovementId(string Value)
{
    public override string ToString() => Value;
}

public readonly record struct VehicleId(string Value)
{
    public override string ToString() => Value;
}

/// <summary>
/// Edges don't have an ID - they are identified by source and destination nodes.
/// </summary>
public readonly record struct EdgeId(NodeId SourceNodeId, NodeId DestinationNodeId) { }
