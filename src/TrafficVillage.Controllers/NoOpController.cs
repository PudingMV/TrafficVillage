using TrafficVillage.Model;

namespace TrafficVillage.Controllers;

/// <summary>
/// Default stub controller that never changes phase. Bartek: replace with real algorithms.
/// </summary>
public sealed class NoOpController : ITrafficController
{
    public ControllerAction Decide(NetworkState state, NodeId intersectionId) =>
        new(ControllerActionKind.KeepPhase);
}
