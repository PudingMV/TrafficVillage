namespace TrafficVillage.Model;

public interface ISimulationManager
{
    SimulationResult Run(
        DemandProfile demand,
        IReadOnlyDictionary<NodeId, ITrafficController> controllers,
        IMetricsCollector metrics,
        SimulationConfig config);
}
