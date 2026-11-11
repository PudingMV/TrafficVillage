using TrafficVillage.Model;

namespace TrafficVillage.Simulation;

/// <summary>
/// Central tick-loop orchestrator. Michał: implement spawning, phases and vehicle movement.
/// </summary>
public sealed class SimulationManager : ISimulationManager
{
    public SimulationResult Run(
        DemandProfile demand,
        IReadOnlyDictionary<NodeId, ITrafficController> controllers,
        IMetricsCollector metrics,
        SimulationConfig config)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(config.TickDurationSeconds);
        ArgumentOutOfRangeException.ThrowIfNegative(config.DurationSeconds);

        var currentTime = 0.0;
        var tickCount = (int)(config.DurationSeconds / config.TickDurationSeconds);

        for (var tick = 0; tick < tickCount; tick++)
        {
            currentTime += config.TickDurationSeconds;

            var state = new NetworkState(
                currentTime,
                [],
                new Dictionary<NodeId, IntersectionState>());

            metrics.OnTick(state);
        }

        return new SimulationResult(
            metrics.Finalize(),
            config.DurationSeconds,
            config.Seed);
    }
}
