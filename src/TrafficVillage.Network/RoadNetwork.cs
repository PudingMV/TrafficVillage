using TrafficVillage.Model;

namespace TrafficVillage.Network;

/// <summary>
/// Stub road network. Eryk: implement graph loading from JSON map files.
/// </summary>
public sealed class RoadNetwork : IRoadNetwork
{
    public IReadOnlyList<NodeId> NodeIds { get; } = Array.Empty<NodeId>();

    public bool TryGetNodeKind(NodeId nodeId, out NodeKind kind)
    {
        kind = NodeKind.Intersection;
        return false;
    }
}
