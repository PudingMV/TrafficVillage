namespace TrafficVillage.Model;

/// <summary>
/// A set of movements that may be green together. Movements in one phase never conflict.
/// </summary>
public sealed record PhaseDefinition(
    PhaseId Id,
    IReadOnlyList<MovementId> GreenMovements,
    double MinGreenSeconds,
    double MaxGreenSeconds);
