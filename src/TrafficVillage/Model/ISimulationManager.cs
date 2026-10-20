namespace TrafficVillage.Model;

public interface ISimulationManager
{
    SimulationResult Run(
        IRoadNetwork network,
        IReadOnlyDictionary<NodeId, ITrafficController> controllers,
        IMetricsCollector metrics,
        int seed,
        double tickDurationSeconds,
        double durationSeconds);
}
