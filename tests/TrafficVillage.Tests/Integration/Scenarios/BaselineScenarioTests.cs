using FluentAssertions;
using TrafficVillage.Core;

namespace TrafficVillage.Tests.Integration.Scenarios;

public class BaselineScenarioTests
{
    [Fact]
    public void Default_baseline_scenario_runs_without_error()
    {
        var orchestrator = new AppOrchestrator();
        var result = orchestrator.RunDefaultScenario();

        result.Should().NotBeNull();
        result.DurationSeconds.Should().Be(100.0);
        result.Seed.Should().Be(42);
        result.Metrics.Should().NotBeNull();
    }
}
