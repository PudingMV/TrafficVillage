using TrafficVillage.Metrics;
using TrafficVillage.Model;
using TrafficVillage.Simulation;
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
    private readonly DemandProfile _demand;
    private readonly IReadOnlyDictionary<NodeId, ITrafficController> _controllers;
    private readonly IMetricsCollector _metrics;
    private readonly SimulationConfig _config;

    public AppOrchestrator(
        ISimulationManager simulation,
        DemandProfile demand,
        IReadOnlyDictionary<NodeId, ITrafficController> controllers,
        IMetricsCollector metrics,
        SimulationConfig config)
    {
        _simulation = simulation;
        _demand = demand;
        _controllers = controllers;
        _metrics = metrics;
        _config = config;
    }

    public static AppOrchestrator CreateDefault() =>
        new(
            new SimulationManager(),
            DemandProfile.Empty,
            new Dictionary<NodeId, ITrafficController>(),
            new NullMetricsCollector(),
            new SimulationConfig(Seed: 42, TickDurationSeconds: 1.0, DurationSeconds: 100.0));

    public static AppOrchestrator CreateFromOptions(CliOptions options) {
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
        name.ToLowerInvariant() switch {
            "noop" => new NoOpController(),
            _ => throw new ArgumentException($"Unknown controller: {name}", nameof(name))
        };

    public SimulationResult Run() =>
        _simulation.Run(_demand, _controllers, _metrics, _config);
}
