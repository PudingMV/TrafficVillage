using FluentAssertions;
using TrafficVillage.Core;

namespace TrafficVillage.Tests.Integration.Scenarios;

public class BaselineScenarioTests
{
    [Fact]
    public void Default_baseline_scenario_runs_without_error()
    {
        var result = AppOrchestrator.CreateDefault().Run();

        result.Should().NotBeNull();
        result.DurationSeconds.Should().Be(100.0);
        result.Seed.Should().Be(42);
        result.Metrics.Should().NotBeNull();
    }
}
