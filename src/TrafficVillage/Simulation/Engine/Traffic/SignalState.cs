using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Engine.Traffic;

/// <summary>
/// State manager for all traffic lights in the simulation.
/// </summary>
// Possible implementation:
// 1. Replace the old single-edge approach and non-existent PhaseState.Red with phase management from
//    IntersectionDefinition (starting with the first phase in DefaultCycle in PhaseState.Green).
// 2. In RequestPhaseChange, validate the requested phase and reject the change if an intermediate
//    state (Yellow/AllRed) is active or MinGreenSeconds has not elapsed.
// 3. In AdvanceTime, implement the Green -> Yellow -> AllRed -> Green state machine, including
//    automatic phase switching once MaxGreenSeconds is exceeded.
// 4. Implement IsMovementGreen (checking if a movement belongs to the active green phase's
//    GreenMovements) and GetIntersectionState (creating an immutable snapshot of the intersection's state).
public sealed class SignalState
{

    /* Old code
    public SignalState(IEnumerable<INode> signalisedNodes)
    {
        _phases = signalisedNodes.ToDictionary(
            n => n.Id,
            n => n.OutgoingEdges.ToDictionary(e => e, _ => PhaseState.Red));
    }*/
    public SignalState(
        IEnumerable<IntersectionDefinition> intersections,
        double yellowDurationSeconds = 3.0,
        double allRedDurationSeconds = 1.0)
    {
        throw new NotImplementedException();
    }

    /* Old code
    public ControllerRequestResult RequestPhaseChange(NodeId sourceNodeId, NodeId destinationNodeId, PhaseState desired)
    {
        if (!_phases.TryGetValue(sourceNodeId, out var nodePhases))
        {
            return ControllerRequestResult.Invalid("Source node not found");
        }

        var outgoingEdge = nodePhases.Keys.FirstOrDefault(edge => edge.To == destinationNodeId);
        if (outgoingEdge == null)
        {
            return ControllerRequestResult.Invalid("Destination node not found");
        }

        var outgoingEdgePhase = nodePhases[outgoingEdge];
        if (outgoingEdgePhase == desired)
        {
            return ControllerRequestResult.AlreadyActive(desired);
        }

        // todo check if state is legal, return ControllerRequestResult.Rejected if not
        // for now method allows to change state even if it conflicts with other states

        nodePhases[outgoingEdge] = desired;
        return ControllerRequestResult.Fulfilled(desired);
    }*/
    public ControllerRequestResult RequestPhaseChange(NodeId intersectionId, PhaseId desiredPhase)
    {
        throw new NotImplementedException();
    }

    public void AdvanceTime(double deltaSeconds)
    {
        throw new NotImplementedException();
    }

    public bool IsMovementGreen(NodeId intersectionId, MovementId movementId)
    {
        throw new NotImplementedException();
    }

    public IntersectionState GetIntersectionState(
        NodeId intersectionId,
        IReadOnlyDictionary<MovementId, int> queueLengths)
    {
        throw new NotImplementedException();
    }
}
