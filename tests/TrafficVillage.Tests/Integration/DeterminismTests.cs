using FluentAssertions;
using TrafficVillage.Core;

namespace TrafficVillage.Tests.Integration;

public class DeterminismTests
{
    [Fact]
    public void Simulation_is_strictly_deterministic_given_same_seed()
    {
        var runA = AppOrchestrator.CreateDefault().Run();
        var runB = AppOrchestrator.CreateDefault().Run();

        runA.DurationSeconds.Should().Be(runB.DurationSeconds);
        runA.Seed.Should().Be(runB.Seed);
        runA.Metrics.Should().BeEquivalentTo(runB.Metrics);
    }
}
