using System.Diagnostics.CodeAnalysis;

namespace TrafficVillage.Model;

public interface IRoadNetwork
{
    public INode? GetNodeById(NodeId id);
    public Edge? GetEdgeByRelatedNodeIds(NodeId source, NodeId destination);

    /// <summary>
    /// Layouts of all intersection nodes, generated once when the network is built.
    /// </summary>
    public IReadOnlyCollection<IntersectionDefinition> Intersections { get; }
    public bool TryGetIntersection(NodeId id, [MaybeNullWhen(false)] out IntersectionDefinition intersection);
}
