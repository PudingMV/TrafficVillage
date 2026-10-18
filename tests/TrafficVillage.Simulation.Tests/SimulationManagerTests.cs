using FluentAssertions;
using TrafficVillage.Controllers;
using TrafficVillage.Metrics;
using TrafficVillage.Model;
using TrafficVillage.Network;
using TrafficVillage.Simulation;

namespace TrafficVillage.Simulation.Tests;

public class SimulationManagerTests
{
    [Fact]
    public void Run_advances_time_for_requested_duration()
    {
        var manager = new SimulationManager();
        var network = new RoadNetwork();
        var controllers = new Dictionary<NodeId, ITrafficController>();
        var metrics = new NullMetricsCollector();

        var result = manager.Run(network, controllers, metrics, 42, 1.0, 100.0);

        result.DurationSeconds.Should().Be(100.0);
        result.Seed.Should().Be(42);
    }
}
