using System.Diagnostics.CodeAnalysis;

namespace TrafficVillage.Model;

public interface IRoadNetwork
{
    public INode? GetNodeById(NodeId id);
    public Edge? GetEdgeByRelatedNodeIds(NodeId source, NodeId destination);
}
