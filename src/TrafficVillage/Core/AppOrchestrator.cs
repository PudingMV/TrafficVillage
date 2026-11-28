using TrafficVillage.Metrics;
using TrafficVillage.Model;
using TrafficVillage.Simulation;
using TrafficVillage.Simulation.Controllers;
using TrafficVillage.Simulation.Engine.RoadNetwork;
using TrafficVillage.Simulation.Controllers;

namespace TrafficVillage.Core;

/// <summary>
/// Composition root — wires modules and runs a simulation scenario end-to-end.
/// Marek: replace CreateDefault with map/demand loading from CLI arguments.
/// </summary>


public sealed record CliOptions(
    string? MapPath,
    string? DemandPath,
    string ControllerName,
    int Seed,
    double TickDurationSeconds,
    double DurationSeconds,
    string? OutputCsvPath
);

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

    public static AppOrchestrator CreateDefault()
    {
        var network = RoadNetwork.FromNodes([]);
        var routes = new RoutePlanner();
        /* for testing
        var controllers = network.Intersections.ToDictionary(
            intersection => intersection.Id,
            ITrafficController (intersection) => new SimpleTimerController(intersection));*/

        return new AppOrchestrator(
            new SimulationManager(),
            network,
            routes,
            DemandProfile.Empty,
            new Dictionary<NodeId, ITrafficController>(), // controllers
            new NullMetricsCollector(),
            new SimulationConfig(Seed: 42, TickDurationSeconds: 1.0, DurationSeconds: 100.0));
    }

    public static readonly IReadOnlySet<string> SupportedControllers =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "noop",
            "simpletimer"
        };

    public static AppOrchestrator CreateFromOptions(CliOptions options)
    {

        if (options.MapPath is not null)
        {
            throw new NotImplementedException("Loading custom maps (--map) is not implemented yet.");
        }

        if (options.DemandPath is not null)
        {
            throw new NotImplementedException("Loading custom demand profiles (--demand) is not implemented yet");
        }

        if (!SupportedControllers.Contains(options.ControllerName))
        {
            throw new ArgumentException(
                $"Unknown controller '{options.ControllerName}'. Supported controllers: {string.Join(", ", SupportedControllers)}.",
                nameof(options));
        }


        var config = new SimulationConfig(
            Seed: options.Seed,
            TickDurationSeconds: options.TickDurationSeconds,
            DurationSeconds: options.DurationSeconds
        );

        var demand = DemandProfile.Empty;

        var controllers = new Dictionary<NodeId, ITrafficController>();

        var metrics = new StandardMetricsCollector(config.TickDurationSeconds);

        return new AppOrchestrator(
            new SimulationManager(),
            demand,
            controllers,
            metrics,
            config
        );
    }

    private static NoOpController CreateController(string name) =>
        name.ToLowerInvariant() switch
        {
            "noop" => new NoOpController(),
            _ => throw new ArgumentException($"Unknown controller: {name}", nameof(name))
        };

    public SimulationResult Run() =>
        _simulation.Run(_network, _routes, _demand, _controllers, _metrics, _config);
}
