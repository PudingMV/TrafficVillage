using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Engine.RoadNetwork;

/// <inheritdoc/>
public sealed class Node : INode
{
    public NodeId Id { get; }
    public NodeKind Kind { get; }
    public IReadOnlyList<Edge> OutgoingEdges { get; }
    public IReadOnlyList<NodeId> ArmOrder { get; }

    public Node(NodeId id, NodeKind kind, IReadOnlyList<Edge> outgoingEdges, IReadOnlyList<NodeId>? armOrder = null)
    {
        Id = id;
        Kind = kind;
        OutgoingEdges = outgoingEdges;
        ArmOrder = armOrder ?? [];
    }

    public Edge? GetOutgoingEdgeByNodeId(NodeId nodeId)
    {
        return OutgoingEdges.FirstOrDefault(edge => edge.To == nodeId);
    }
}
