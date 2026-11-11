namespace TrafficVillage.Model;

/// <summary>
/// Type of node
/// </summary>
public enum NodeKind
{
    /// <summary>
    /// Map entry/exit point. May be used only to start/end journey.
    /// </summary>
    Entry,
    /// <summary>
    /// Intersection node. May be used to start/end journey as well as to continue from one road (called edge) to another.
    /// </summary>
    Intersection
}

public enum VehicleState
{
    /// <summary>
    /// Vehicle is moving - it is currently on specific Edge and has more than 0 ticks remaining to be able to leave it.
    /// </summary>
    Moving,
    /// <summary>
    /// Vehicle is queued at intersection and waiting for its turn to cross it.
    /// </summary>
    Queued,

    /// <summary>
    /// Vehicle has reached its destination..
    /// </summary>
    Finished
}

/// <summary>
/// State of traffic phase on intersections with traffic lights
/// </summary>
public enum PhaseState
{
    /// <summary>
    /// Traffic ongoing
    /// </summary>
    Green,

    /// <summary>
    /// Traffic stopped
    /// </summary>
    Red
}

/// <summary>
/// Result of controller request
/// </summary>
public enum ControllerRequestResultState
{
    /// <summary>
    /// Request was already fulfilled - no state change required
    /// </summary>
    AlreadyActive,

    /// <summary>
    /// Request was fulfilled
    /// </summary>
    Fulfilled,

    /// <summary>
    /// Request was not fulfilled, in example because of state conflicts
    /// </summary>
    Rejected,

    /// <summary>
    /// Error: invalid request
    /// </summary>
    Invalid
}
