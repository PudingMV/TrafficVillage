using FluentAssertions;
using TrafficVillage.Metrics;
using TrafficVillage.Model;

namespace TrafficVillage.Tests.Metrics;

public class NullMetricsCollectorTests
{
    [Fact]
    public void Finalize_returns_zero_metrics()
    {
        var collector = new NullMetricsCollector();
        var state = new NetworkState(
            0,
            [],
            new Dictionary<NodeId, IntersectionState>());

        collector.OnTick(state);
        var metrics = collector.Finalize();

        metrics.TotalVehiclesExited.Should().Be(0);
        metrics.AverageWaitTimeSeconds.Should().Be(0);
    }
}
