using TrafficVillage.Model;

namespace TrafficVillage.Metrics;

/// <summary>
/// Stub metrics collector. Marek: implement real metric tracking.
/// </summary>
public sealed class NullMetricsCollector : IMetricsCollector
{
    public void OnTick(NetworkState state)
    {
    }

    public void OnVehicleExited(Vehicle vehicle)
    {
    }

    public SimulationMetrics Finalize() =>
        new(
            AverageWaitTimeSeconds: 0,
            P95WaitTimeSeconds: 0,
            TotalVehiclesExited: 0,
            ThroughputPerHour: 0);
}
