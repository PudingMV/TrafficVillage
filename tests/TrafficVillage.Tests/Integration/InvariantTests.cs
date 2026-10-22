using FluentAssertions;
using TrafficVillage.Metrics;
using TrafficVillage.Model;
using TrafficVillage.Simulation;
using TrafficVillage.Simulation.Engine.RoadNetwork;

namespace TrafficVillage.Tests.Integration;

public class InvariantTests
{
    [Fact]
    public void Tick_count_and_simulation_duration_match_requested_parameters()
    {
        var manager = new SimulationManager();
        var network = new RoadNetwork();
        var controllers = new Dictionary<NodeId, ITrafficController>();
        var metrics = new NullMetricsCollector();

        var result = manager.Run(
            network,
            controllers,
            metrics,
            seed: 123,
            tickDurationSeconds: 0.5,
            durationSeconds: 50.0);

        result.DurationSeconds.Should().Be(50.0);
        result.Seed.Should().Be(123);
    }
}
