using TrafficVillage.Model;

namespace TrafficVillage.Simulation;

/// <summary>
/// Central tick-loop orchestrator. Michał: implement spawning, phases and vehicle movement.
/// </summary>
public sealed class SimulationManager : ISimulationManager
{
    // Possible implementation:
    // 1. Before the loop, initialize a config.Seed, a SignalState instance for network.Intersections,
    //    and separate FIFO queues for each MovementId.
    // 2. On each tick, spawn new vehicles based on demand and routes, then build the current
    //    NetworkState snapshot.
    // 3. Query controllers (controller.Decide) for each intersection, forward phase change requests
    //    to SignalState, and advance signal timers.
    // 4. Update vehicle positions on edges and in queues (calling metrics.OnVehicleExited upon
    //    reaching the destination) before passing the snapshot to metrics.OnTick.
    public SimulationResult Run(
        IRoadNetwork network,
        IRoutePlanner routes,
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
