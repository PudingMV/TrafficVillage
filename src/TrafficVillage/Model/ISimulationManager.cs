namespace TrafficVillage.Model;

public interface ISimulationManager
{
    SimulationResult Run(
        IRoadNetwork network,
        IRoutePlanner routes,
        DemandProfile demand,
        IReadOnlyDictionary<NodeId, ITrafficController> controllers,
        IMetricsCollector metrics,
        SimulationConfig config);
}
