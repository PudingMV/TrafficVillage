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
    /// <summary>
    /// Neighbouring nodes in clockwise order around this node (any starting arm).
    /// Empty when unknown; intersections with 3+ arms then fall back to one phase per approach.
    /// </summary>
    IReadOnlyList<NodeId> ArmOrder { get; }
}
