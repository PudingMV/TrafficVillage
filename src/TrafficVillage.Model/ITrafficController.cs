namespace TrafficVillage.Model;

/// <summary>
/// Pluggable traffic light control algorithm.
/// Implementations receive read-only snapshots and return actions — never mutate state directly.
/// </summary>
public interface ITrafficController
{
    ControllerAction Decide(NetworkState state, NodeId intersectionId);
}
