using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Engine.RoadNetwork;

/// <summary>
/// Stub route planner. Eryk: implement shortest path (Dijkstra) over IRoadNetwork.
/// </summary>
public sealed class RoutePlanner : IRoutePlanner
{
    public IReadOnlyList<EdgeId> FindRoute(NodeId origin, NodeId destination) => [];
}
