namespace TrafficVillage.Model;

public sealed record SimulationConfig(
    int Seed,
    double TickDurationSeconds,
    double DurationSeconds);
