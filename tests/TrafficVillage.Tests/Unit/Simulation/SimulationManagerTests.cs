using FluentAssertions;
using TrafficVillage.Metrics;
using TrafficVillage.Model;
using TrafficVillage.Simulation;
using TrafficVillage.Simulation.Engine.RoadNetwork;

namespace TrafficVillage.Tests.Simulation;

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

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    public void Run_throws_when_tick_duration_is_zero_or_negative(double tickDuration)
    {
        var manager = new SimulationManager();
        var network = new RoadNetwork();
        var controllers = new Dictionary<NodeId, ITrafficController>();
        var metrics = new NullMetricsCollector();

        var act = () => manager.Run(network, controllers, metrics, 42, tickDuration, 100.0);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Run_throws_when_duration_is_negative()
    {
        var manager = new SimulationManager();
        var network = new RoadNetwork();
        var controllers = new Dictionary<NodeId, ITrafficController>();
        var metrics = new NullMetricsCollector();

        var act = () => manager.Run(network, controllers, metrics, 42, 1.0, -10.0);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
