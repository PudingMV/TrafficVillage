namespace TrafficVillage.Model;

/// <summary>
/// Static layout of a signalised intersection. Controllers may only switch to phases listed here.
/// </summary>
/// <param name="Phases">Every maximal conflict-free set of movements.</param>
/// <param name="DefaultCycle">Smallest phase sequence that gives every movement green at least once.</param>
public sealed record IntersectionDefinition(
    NodeId Id,
    IReadOnlyList<Movement> Movements,
    IReadOnlyList<PhaseDefinition> Phases,
    IReadOnlyList<PhaseId> DefaultCycle);
