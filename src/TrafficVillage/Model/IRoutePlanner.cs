namespace TrafficVillage.Model;

public interface IRoutePlanner
{
    IReadOnlyList<EdgeId> FindRoute(NodeId origin, NodeId destination);
}
