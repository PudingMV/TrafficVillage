using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Engine.RoadNetwork;

/// <inheritdoc/>
public sealed class Node : INode
{
    private readonly Dictionary<Edge, PhaseState> _currentPhases;
    public NodeId Id { get; }
    public NodeKind Kind { get; }
    public IReadOnlyList<Edge> OutgoingEdges { get; }
    public bool HasTrafficLight { get; }
    public IReadOnlyDictionary<Edge, PhaseState> CurrentPhases { get; }

    public Node(NodeId id, NodeKind kind, IReadOnlyList<Edge> outgoingEdges, bool hasTrafficLight)
    {
        Id = id;
        Kind = kind;
        OutgoingEdges = outgoingEdges;
        HasTrafficLight = hasTrafficLight;


        // todo consider separating traffic lights and non-traffic lights Nodes into distinct 
        // objects with shared (inherited) parent
        if (hasTrafficLight)
        {
            _currentPhases = OutgoingEdges.ToDictionary(edge => edge, edge => PhaseState.Red); // start with all red
        }
        else
        {
            _currentPhases = [];
        }

        CurrentPhases = _currentPhases.AsReadOnly();
    }

    public ControllerRequestResult RequestPhaseChange(NodeId outgoingNodeId, PhaseState desiredPhaseState)
    {
        if (!HasTrafficLight)
        {
            return ControllerRequestResult.Invalid("Node has no traffic light");
        }

        var outgoingEdge = GetOutgoingEdgeByNodeId(outgoingNodeId);
        if (outgoingEdge == null)
        {
            return ControllerRequestResult.Invalid("Outgoing node not found");
        }

        PhaseState? outgoingEdgePhase = CurrentPhases.GetValueOrDefault(outgoingEdge);
        if (outgoingEdgePhase == null)
        {
            throw new InvalidDataException("Illegal state: traffic phase for edge does not exist.");
        }

        if (outgoingEdgePhase == desiredPhaseState)
        {
            return ControllerRequestResult.AlreadyActive(desiredPhaseState);
        }

        // todo check if state is legal, return ControllerRequestResult.Rejected if not
        // for now method allows to change state even if it conflicts with other states

        _currentPhases[outgoingEdge] = desiredPhaseState;
        return ControllerRequestResult.Fulfilled(desiredPhaseState);
    }

    public Edge? GetOutgoingEdgeByNodeId(NodeId nodeId)
    {
        return OutgoingEdges.FirstOrDefault(edge => edge.To == nodeId);
    }


}

