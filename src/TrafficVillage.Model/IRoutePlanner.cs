namespace TrafficVillage.Model;

public interface IRoutePlanner
{
    IReadOnlyList<EdgeId> FindRoute(NodeId from, NodeId to);
}
