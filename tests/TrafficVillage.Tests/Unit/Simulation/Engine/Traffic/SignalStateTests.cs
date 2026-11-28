using FluentAssertions;
using TrafficVillage.Model;
using TrafficVillage.Simulation.Engine.RoadNetwork;
using TrafficVillage.Simulation.Engine.Traffic;
using TrafficVillage.Tests.Fakes;

namespace TrafficVillage.Tests.Unit.Simulation.Engine.Traffic;

public class SignalStateTests
{
    /* TODO reimplement in new API with Phases
    [Fact]
    public void RequestPhaseChange_fulfills_and_persists_new_state()
    {
        var nodeA = new NodeId("A");
        var nodeB = new NodeId("B");
        var edge = new Edge(nodeA, nodeB, 10);
        var node = new Node(nodeA, NodeKind.Intersection, [edge]);

        var signalState = new SignalState([node]);

        var fulfillResult = signalState.RequestPhaseChange(nodeA, nodeB, PhaseState.Green);
        fulfillResult.State.Should().Be(ControllerRequestResultState.Fulfilled);
        fulfillResult.CurrentState.Should().Be(PhaseState.Green);

        var repeatResult = signalState.RequestPhaseChange(nodeA, nodeB, PhaseState.Green);
        repeatResult.State.Should().Be(ControllerRequestResultState.AlreadyActive);
        repeatResult.CurrentState.Should().Be(PhaseState.Green);
    }

    [Fact]
    public void RequestPhaseChange_returns_already_active_when_state_matches()
    {
        var nodeA = new NodeId("A");
        var nodeB = new NodeId("B");
        var edge = new Edge(nodeA, nodeB, 10);
        var node = new Node(nodeA, NodeKind.Intersection, [edge]);

        var signalState = new SignalState([node]);

        var result = signalState.RequestPhaseChange(nodeA, nodeB, PhaseState.AllRed);

        result.State.Should().Be(ControllerRequestResultState.AlreadyActive);
        result.CurrentState.Should().Be(PhaseState.AllRed);
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
    }*/

    [Fact]
    public void Stub_throws_not_implemented()
    {
        var fake = new FakeFourWay();

        // shortcut: stub throws NotImplementedException, replace with phase transition assertions once implemented
        var act = () => new SignalState([fake.Intersection]);

        act.Should().Throw<NotImplementedException>();
    }
}
