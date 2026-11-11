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

    public void OnVehicleExited(IVehicle vehicle)
    {
    }

    public SimulationMetrics Finalize() =>
        new(
            AverageWaitTimeSeconds: 0,
            P95WaitTimeSeconds: 0,
            MaxQueueLength: 0,
            TotalVehiclesExited: 0,
            ThroughputPerHour: 0,
            AverageStopCount: 0);
}
