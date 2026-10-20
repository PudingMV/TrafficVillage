namespace TrafficVillage.Model;

public sealed record ControllerAction(
    ControllerActionKind Kind,
    PhaseId? TargetPhase = null);
