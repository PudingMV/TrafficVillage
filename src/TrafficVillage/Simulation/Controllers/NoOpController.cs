using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Controllers;

/// <summary>
/// Default stub controller that never changes phase. Bartek: replace with real algorithms.
/// </summary>
public sealed class NoOpController : ITrafficController
{

    public ControllerRequest Decide(NetworkState state, NodeId intersectionId)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (!state.Intersections.TryGetValue(intersectionId, out var intersection))
        {
            throw new KeyNotFoundException(
                $"Intersection '{intersectionId}' was not found in the current network state.");
        }

        return new ControllerRequest(intersection.CurrentPhase);
    }
}
