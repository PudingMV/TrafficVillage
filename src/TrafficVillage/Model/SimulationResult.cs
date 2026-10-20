namespace TrafficVillage.Model;

public sealed record SimulationResult(
    SimulationMetrics Metrics,
    double DurationSeconds,
    int Seed);
