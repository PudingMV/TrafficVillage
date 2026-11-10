namespace TrafficVillage.Model;

public sealed record IntersectionState(
    NodeId Id,
    PhaseId CurrentPhase,
    PhaseState PhaseState,
    double TimeInPhaseSeconds,
    IReadOnlyDictionary<MovementId, int> QueueLengths);
