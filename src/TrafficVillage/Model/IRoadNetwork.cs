namespace TrafficVillage.Model;

public interface IRoadNetwork
{
    IReadOnlyList<NodeId> NodeIds { get; }

    bool TryGetNodeKind(NodeId nodeId, out NodeKind kind);
}
