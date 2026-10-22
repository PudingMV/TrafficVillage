namespace TrafficVillage.Model;

public sealed record SimulationMetrics(
    double AverageWaitTimeSeconds,
    double P95WaitTimeSeconds,
    int TotalVehiclesExited,
    double ThroughputPerHour);
