using FluentAssertions;
using TrafficVillage.Cli;

namespace TrafficVillage.Integration.Tests;

public class SolutionIntegrationTests
{
    [Fact]
    public void AppOrchestrator_runs_default_scenario_without_error()
    {
        var orchestrator = new AppOrchestrator();
        var result = orchestrator.RunDefaultScenario();

        result.DurationSeconds.Should().Be(100.0);
        result.Seed.Should().Be(42);
        result.Metrics.Should().NotBeNull();
    }
}
