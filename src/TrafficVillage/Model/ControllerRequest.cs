namespace TrafficVillage.Model;

/// <summary>
/// Communication class used by Controller to request a phase change from Engine.
/// </summary>
public sealed record ControllerRequest(
    EdgeId EdgeId,
    PhaseState DesiredPhase);
