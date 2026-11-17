using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Engine.Traffic;

/// <summary>
/// State manager for all traffic lights in the simulation.
/// </summary>
public sealed class SignalState
{
    /// <summary>
    /// Current traffic lights state
    /// </summary>
    private readonly Dictionary<NodeId, Dictionary<Edge, PhaseState>> _phases;

    public SignalState(IEnumerable<INode> signalisedNodes)
    {
        _phases = signalisedNodes.ToDictionary(
            n => n.Id,
            n => n.OutgoingEdges.ToDictionary(e => e, _ => PhaseState.Red));
    }

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
    }
}

