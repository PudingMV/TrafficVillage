using TrafficVillage.Model;

namespace TrafficVillage.Simulation;

/// <summary>
/// Central tick-loop orchestrator. Michał: implement full simulation logic.
/// </summary>
public sealed class SimulationManager : ISimulationManager
{
    public SimulationResult Run(
        IRoadNetwork network,
        IReadOnlyDictionary<NodeId, ITrafficController> controllers,
        IMetricsCollector metrics,
        int seed,
        double tickDurationSeconds,
        double durationSeconds)
    {
        ArgumentNullException.ThrowIfNull(network);
        ArgumentNullException.ThrowIfNull(controllers);
        ArgumentNullException.ThrowIfNull(metrics);

        var currentTime = 0.0;
        var tickCount = (int)(durationSeconds / tickDurationSeconds);

        for (var tick = 0; tick < tickCount; tick++)
        {
            currentTime += tickDurationSeconds;

            var state = new NetworkState(
                currentTime,
                Array.Empty<Vehicle>(),
                new Dictionary<NodeId, IntersectionState>());

            metrics.OnTick(state);
        }

        return new SimulationResult(
            metrics.Finalize(),
            durationSeconds,
            seed);
    }
}
