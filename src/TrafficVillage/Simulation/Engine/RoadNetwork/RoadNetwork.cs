using System.Diagnostics.CodeAnalysis;
using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Engine.RoadNetwork;

/// <summary>
/// Stub road network. Eryk: build from a loaded map and generate intersection phases.
/// </summary>
public sealed class RoadNetwork : IRoadNetwork
{
    public IReadOnlyList<Node> Nodes { get; } = [];

    public IReadOnlyList<Edge> Edges { get; } = [];

    public IReadOnlyList<NodeId> ControlledIntersectionIds { get; } = [];

    public bool TryGetNode(NodeId id, [MaybeNullWhen(false)] out Node node)
    {
        node = null;
        return false;
    }

    public bool TryGetEdge(EdgeId id, [MaybeNullWhen(false)] out Edge edge)
    {
        edge = null;
        return false;
    }

    public IReadOnlyList<Edge> IncomingEdges(NodeId nodeId) => [];

    public IReadOnlyList<Edge> OutgoingEdges(NodeId nodeId) => [];

    public bool TryGetIntersection(NodeId id, [MaybeNullWhen(false)] out IntersectionDefinition intersection)
    {
        intersection = null;
        return false;
    }
}
