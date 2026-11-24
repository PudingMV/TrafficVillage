using System.Diagnostics.CodeAnalysis;
using TrafficVillage.Model;
using TrafficVillage.Simulation.Engine.RoadNetwork.Intersections;

namespace TrafficVillage.Simulation.Engine.RoadNetwork;

/// <summary>
/// Road network. Eryk: build from a loaded map. Intersection phases are generated on construction.
/// </summary>
public sealed class RoadNetwork : IRoadNetwork
{
    private readonly Dictionary<NodeId, IntersectionDefinition> _intersections;

    public IReadOnlyDictionary<NodeId, Node> Nodes { get; }

    public IReadOnlyCollection<IntersectionDefinition> Intersections => _intersections.Values;

    private RoadNetwork(IReadOnlyList<Node> nodes)
    {
        Nodes = nodes.ToDictionary(node => node.Id);
        _intersections = IntersectionBuilder.BuildAll(nodes);
    }

    public static RoadNetwork FromNodes(IReadOnlyList<Node> nodes) => new(nodes);

    public static RoadNetwork GenerateRandom(int numberOfNodes)
    {
        // todo Eryk
        throw new NotImplementedException();
    }

    public static RoadNetwork FromMap()
    {
        // todo Eryk: parse the map, then return FromNodes(nodes)
        throw new NotImplementedException();
    }

    public INode? GetNodeById(NodeId id)
    {
        return Nodes.GetValueOrDefault(id);
    }

    public Edge? GetEdgeByRelatedNodeIds(NodeId source, NodeId destination)
    {
        return GetNodeById(source)?.OutgoingEdges.FirstOrDefault(edge => edge.To == destination);
    }

    public bool TryGetIntersection(NodeId id, [MaybeNullWhen(false)] out IntersectionDefinition intersection) =>
        _intersections.TryGetValue(id, out intersection);
}
