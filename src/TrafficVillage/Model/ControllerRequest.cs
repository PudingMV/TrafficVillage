namespace TrafficVillage.Model;

/// <summary>
/// Communication record used by <see cref="ITrafficController"/> to request a target phase from Engine.
/// Controllers may only request <see cref="PhaseId"/> values defined in <see cref="IntersectionDefinition.Phases"/>.
/// </summary>
public sealed record ControllerRequest(PhaseId DesiredPhase);
