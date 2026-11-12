namespace TrafficVillage.Model;

/// <summary>
/// Node represents a junction in the map. It can be a start/end of a segment or an intersection of multiple segments.
/// </summary>
public interface INode
{
    NodeId Id { get; }
    NodeKind Kind { get; }
    /// <summary>
    /// Represents outgoing connections. Incoming connections must be inferred from source nodes.
    /// </summary>
    IReadOnlyList<Edge> OutgoingEdges { get; }
}

