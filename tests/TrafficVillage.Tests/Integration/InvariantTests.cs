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
        var result = new SimulationManager().Run(
            new RoadNetwork(),
            new RoutePlanner(),
            DemandProfile.Empty,
            new Dictionary<NodeId, ITrafficController>(),
            new NullMetricsCollector(),
            new SimulationConfig(Seed: 123, TickDurationSeconds: 0.5, DurationSeconds: 50.0));

        result.DurationSeconds.Should().Be(50.0);
        result.Seed.Should().Be(123);
    }
}
