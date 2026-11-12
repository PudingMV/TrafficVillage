using FluentAssertions;
using TrafficVillage.Model;
using TrafficVillage.Simulation.Engine.RoadNetwork;
using TrafficVillage.Simulation.Engine.Traffic;

namespace TrafficVillage.Tests.Unit.Simulation.Engine.Traffic;

public class SignalStateTests
{
    [Fact]
    public void Constructor_initializes_all_phases_to_red()
    {
        var nodeA = new NodeId("A");
        var nodeB = new NodeId("B");
        var edge = new Edge(nodeA, nodeB, 10);
        var node = new Node(nodeA, NodeKind.Intersection, [edge]);

        var signalState = new SignalState([node]);
        var snapshot = signalState.Snapshot(nodeA);

        snapshot.Should().ContainSingle();
        snapshot[edge].Should().Be(PhaseState.Red);
    }

    [Fact]
    public void Snapshot_returns_independent_copy()
    {
        var nodeA = new NodeId("A");
        var nodeB = new NodeId("B");
        var edge = new Edge(nodeA, nodeB, 10);
        var node = new Node(nodeA, NodeKind.Intersection, [edge]);

        var signalState = new SignalState([node]);
        var firstSnapshot = signalState.Snapshot(nodeA);

        var result = signalState.RequestPhaseChange(nodeA, nodeB, PhaseState.Green);
        result.State.Should().Be(ControllerRequestResultState.Fulfilled);

        firstSnapshot[edge].Should().Be(PhaseState.Red);
        signalState.Snapshot(nodeA)[edge].Should().Be(PhaseState.Green);
    }

    [Fact]
    public void RequestPhaseChange_returns_already_active_when_state_matches()
    {
        var nodeA = new NodeId("A");
        var nodeB = new NodeId("B");
        var edge = new Edge(nodeA, nodeB, 10);
        var node = new Node(nodeA, NodeKind.Intersection, [edge]);

        var signalState = new SignalState([node]);

        var result = signalState.RequestPhaseChange(nodeA, nodeB, PhaseState.Red);

        result.State.Should().Be(ControllerRequestResultState.AlreadyActive);
        result.CurrentState.Should().Be(PhaseState.Red);
    }

    [Fact]
    public void RequestPhaseChange_returns_invalid_for_unknown_node_or_outgoing_node()
    {
        var nodeA = new NodeId("A");
        var nodeB = new NodeId("B");
        var edge = new Edge(nodeA, nodeB, 10);
        var node = new Node(nodeA, NodeKind.Intersection, [edge]);

        var signalState = new SignalState([node]);

        var invalidNodeResult = signalState.RequestPhaseChange(new NodeId("UNKNOWN"), nodeB, PhaseState.Green);
        invalidNodeResult.State.Should().Be(ControllerRequestResultState.Invalid);

        var invalidOutgoingResult = signalState.RequestPhaseChange(nodeA, new NodeId("UNKNOWN"), PhaseState.Green);
        invalidOutgoingResult.State.Should().Be(ControllerRequestResultState.Invalid);
    }
}

