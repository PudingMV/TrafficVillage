using FluentAssertions;
using TrafficVillage.Metrics;
using TrafficVillage.Model;
using TrafficVillage.Simulation;
using TrafficVillage.Tests.Fakes;

namespace TrafficVillage.Tests.Simulation;

public class SimulationManagerTests
{
    private static SimulationResult Run(SimulationConfig config)
    {
        var network = new FakeFourWay();

        return new SimulationManager().Run(
            network,
            network,
            DemandProfile.Empty,
            new Dictionary<NodeId, ITrafficController>(),
            new NullMetricsCollector(),
            config);
    }

    [Fact]
    public void Run_advances_time_for_requested_duration()
    {
        var result = Run(new SimulationConfig(Seed: 42, TickDurationSeconds: 1.0, DurationSeconds: 100.0));

        result.DurationSeconds.Should().Be(100.0);
        result.Seed.Should().Be(42);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    public void Run_throws_when_tick_duration_is_zero_or_negative(double tickDuration)
    {
        var act = () => Run(new SimulationConfig(42, tickDuration, 100.0));

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Run_throws_when_duration_is_negative()
    {
        var act = () => Run(new SimulationConfig(42, 1.0, -10.0));

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
