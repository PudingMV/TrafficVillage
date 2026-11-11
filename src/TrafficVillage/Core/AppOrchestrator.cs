using TrafficVillage.Metrics;
using TrafficVillage.Model;
using TrafficVillage.Simulation;
using TrafficVillage.Simulation.Engine.RoadNetwork;

namespace TrafficVillage.Core;

/// <summary>
/// Composition root — wires modules and runs a simulation scenario end-to-end.
/// Marek: replace CreateDefault with map/demand loading from CLI arguments.
/// </summary>
public sealed class AppOrchestrator
{
    private readonly ISimulationManager _simulation;
    private readonly IRoadNetwork _network;
    private readonly IRoutePlanner _routes;
    private readonly DemandProfile _demand;
    private readonly IReadOnlyDictionary<NodeId, ITrafficController> _controllers;
    private readonly IMetricsCollector _metrics;
    private readonly SimulationConfig _config;

    public AppOrchestrator(
        ISimulationManager simulation,
        IRoadNetwork network,
        IRoutePlanner routes,
        DemandProfile demand,
        IReadOnlyDictionary<NodeId, ITrafficController> controllers,
        IMetricsCollector metrics,
        SimulationConfig config)
    {
        _simulation = simulation;
        _network = network;
        _routes = routes;
        _demand = demand;
        _controllers = controllers;
        _metrics = metrics;
        _config = config;
    }

    public static AppOrchestrator CreateDefault() =>
        new(
            new SimulationManager(),
            new RoadNetwork(),
            new RoutePlanner(),
            DemandProfile.Empty,
            new Dictionary<NodeId, ITrafficController>(),
            new NullMetricsCollector(),
            new SimulationConfig(Seed: 42, TickDurationSeconds: 1.0, DurationSeconds: 100.0));

    public SimulationResult Run() =>
        _simulation.Run(_network, _routes, _demand, _controllers, _metrics, _config);
}
