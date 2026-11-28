using TrafficVillage.Model;

namespace TrafficVillage.Metrics;

public sealed class StandardMetricsCollector : IMetricsCollector
{
    private readonly double _tickDurationSeconds;
    private readonly List<int> _exitedStopCounts = [];
    private readonly List<double> _exitedWaitTimesSeconds = [];

    private readonly Dictionary<VehicleId, int> _lastWaitTicks = [];
    private readonly Dictionary<VehicleId, bool> _wasWaiting = [];
    private readonly Dictionary<VehicleId, int> _activeStopCounts = [];

    private int _maxQueueLength;
    private double _lastTimeSeconds;

    public StandardMetricsCollector(double tickDurationSeconds)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(tickDurationSeconds);
        _tickDurationSeconds = tickDurationSeconds;
    }

    public void OnTick(NetworkState state)
    {
        _lastTimeSeconds = state.CurrentTimeSeconds;

        foreach (var intersection in state.Intersections.Values)
        {
            if (intersection.QueueLengths.Count > 0)
            {
                int maxInIntersection = intersection.QueueLengths.Values.Max();
                if (maxInIntersection > _maxQueueLength)
                    _maxQueueLength = maxInIntersection;
            }
        }

        foreach (var vehicle in state.Vehicles)
        {
            int prevWait = _lastWaitTicks.GetValueOrDefault(vehicle.Id, 0);
            bool isWaitingNow = vehicle.WaitTime > prevWait;
            bool wasWaitingBefore = _wasWaiting.GetValueOrDefault(vehicle.Id, false);

            if (isWaitingNow && !wasWaitingBefore)
                _activeStopCounts[vehicle.Id] = _activeStopCounts.GetValueOrDefault(vehicle.Id, 0) + 1;

            _lastWaitTicks[vehicle.Id] = vehicle.WaitTime;
            _wasWaiting[vehicle.Id] = isWaitingNow;
        }
    }

    public void OnVehicleExited(IVehicle vehicle)
    {
        _exitedWaitTimesSeconds.Add(vehicle.WaitTime * _tickDurationSeconds);
        _exitedStopCounts.Add(_activeStopCounts.GetValueOrDefault(vehicle.Id, 0));

        _lastWaitTicks.Remove(vehicle.Id);
        _wasWaiting.Remove(vehicle.Id);
        _activeStopCounts.Remove(vehicle.Id);
    }

    public SimulationMetrics Finalize()
    {
        int exitedCount = _exitedWaitTimesSeconds.Count;
        double simulatedHours = _lastTimeSeconds / 3600.0;

        double avgWait, p95Wait, avgStops, throughput;

        if (exitedCount > 0)
        {
            avgWait = _exitedWaitTimesSeconds.Average();
            p95Wait = CalculatePercentile(_exitedWaitTimesSeconds, 0.95);
            avgStops = _exitedStopCounts.Average();
        }
        else
        {
            avgWait = 0.0;
            p95Wait = 0.0;
            avgStops = 0.0;
        }

        if (simulatedHours > 0)
            throughput = exitedCount / simulatedHours;
        else
            throughput = 0.0;

        return new SimulationMetrics(
            AverageWaitTimeSeconds: Math.Round(avgWait, 2),
            P95WaitTimeSeconds: Math.Round(p95Wait, 2),
            MaxQueueLength: _maxQueueLength,
            TotalVehiclesExited: exitedCount,
            ThroughputPerHour: Math.Round(throughput, 2),
            AverageStopCount: Math.Round(avgStops, 2)
        );
    }

    private static double CalculatePercentile(List<double> values, double percentile)
    {
        var sorted = values.OrderBy(v => v).ToList();
        int index = (int)Math.Ceiling(percentile * sorted.Count) - 1;
        return sorted[Math.Clamp(index, 0, sorted.Count - 1)];
    }
}
