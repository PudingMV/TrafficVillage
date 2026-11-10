using System.Diagnostics.CodeAnalysis;

namespace TrafficVillage.Model;

public interface IRoadNetwork
{
    IReadOnlyList<Node> Nodes { get; }

    IReadOnlyList<Edge> Edges { get; }

    IReadOnlyList<NodeId> ControlledIntersectionIds { get; }

    bool TryGetNode(NodeId id, [MaybeNullWhen(false)] out Node node);

    bool TryGetEdge(EdgeId id, [MaybeNullWhen(false)] out Edge edge);

    IReadOnlyList<Edge> IncomingEdges(NodeId nodeId);

    IReadOnlyList<Edge> OutgoingEdges(NodeId nodeId);

    bool TryGetIntersection(NodeId id, [MaybeNullWhen(false)] out IntersectionDefinition intersection);
}
