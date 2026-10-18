using TrafficVillage.Controllers;
using TrafficVillage.Metrics;
using TrafficVillage.Model;
using TrafficVillage.Network;
using TrafficVillage.Simulation;

namespace TrafficVillage.Cli;

/// <summary>
/// Composition root — wires modules and runs a simulation scenario end-to-end.
/// Marek: extend with map/demand loading and result export.
/// </summary>
public sealed class AppOrchestrator
{
    private readonly ISimulationManager _simulationManager;

    public AppOrchestrator(ISimulationManager? simulationManager = null)
    {
        _simulationManager = simulationManager ?? new SimulationManager();
    }

    public SimulationResult RunDefaultScenario()
    {
        var network = new RoadNetwork();
        var controllers = new Dictionary<NodeId, ITrafficController>();
        var metrics = new NullMetricsCollector();

        return _simulationManager.Run(
            network,
            controllers,
            metrics,
            seed: 42,
            tickDurationSeconds: 1.0,
            durationSeconds: 100.0);
    }
}
