namespace TrafficVillage.Model;

public sealed record SimulationMetrics(
    double AverageWaitTimeSeconds,
    double P95WaitTimeSeconds,
    int MaxQueueLength,
    int TotalVehiclesExited,
    double ThroughputPerHour,
    double AverageStopCount);
