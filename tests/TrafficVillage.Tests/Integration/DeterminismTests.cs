using FluentAssertions;
using TrafficVillage.Core;

namespace TrafficVillage.Tests.Integration;

public class DeterminismTests
{
    [Fact]
    public void Simulation_is_strictly_deterministic_given_same_seed()
    {
        var orchestrator = new AppOrchestrator();

        var runA = orchestrator.RunDefaultScenario();
        var runB = orchestrator.RunDefaultScenario();

        runA.DurationSeconds.Should().Be(runB.DurationSeconds);
        runA.Seed.Should().Be(runB.Seed);
        runA.Metrics.Should().BeEquivalentTo(runB.Metrics);
    }
}
