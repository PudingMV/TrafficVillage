namespace TrafficVillage.Model;

/// <summary>
/// Static layout of a signalised intersection. Controllers may only switch to phases listed here.
/// </summary>
public sealed record IntersectionDefinition(
    NodeId Id,
    IReadOnlyList<Movement> Movements,
    IReadOnlyList<PhaseDefinition> Phases);
