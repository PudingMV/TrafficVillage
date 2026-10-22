namespace TrafficVillage.Model;

public sealed record IntersectionState(
    NodeId Id,
    PhaseId CurrentPhase,
    PhaseState PhaseState,
    double TimeInPhaseSeconds,
    IReadOnlyDictionary<string, int> QueueLengthsByApproach);
