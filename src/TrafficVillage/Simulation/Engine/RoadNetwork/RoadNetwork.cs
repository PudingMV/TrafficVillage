using System.Diagnostics.CodeAnalysis;
using System.Xml;
using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Engine.RoadNetwork;

/// <summary>
/// Stub road network. Eryk: build from a loaded map and generate intersection phases.
/// </summary>
public sealed class RoadNetwork : IRoadNetwork
{
    public IReadOnlyDictionary<NodeId, Node> Nodes { get; }

    private RoadNetwork(IReadOnlyList<Node> nodes)
    {
        Nodes = nodes.ToDictionary(node => node.Id);
    }

    public static RoadNetwork GenerateRandom(int numberOfNodes)
    {
        // todo Eryk
        throw new NotImplementedException();
    }

    public static RoadNetwork FromMap()
    {
        // todo Eryk
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

}
