using FluentAssertions;
using TrafficVillage.Model;

namespace TrafficVillage.Tests.Model;

public class ModelContractsTests
{
    [Fact]
    public void ControllerRequestResult_fulfilled_sets_current_state()
    {
        var result = ControllerRequestResult.Fulfilled(PhaseState.Green);

        result.State.Should().Be(ControllerRequestResultState.Fulfilled);
        result.CurrentState.Should().Be(PhaseState.Green);
        result.Reason.Should().BeNull();
    }

    [Fact]
    public void NetworkState_can_be_created_with_empty_collections()
    {
        var state = new NetworkState(0, [], new Dictionary<NodeId, IntersectionState>());

        state.CurrentTimeSeconds.Should().Be(0);
        state.Vehicles.Should().BeEmpty();
        state.Intersections.Should().BeEmpty();
    }

    [Fact]
    public void IntersectionState_tracks_queues_per_movement()
    {
        var movement = new MovementId("N-A-S");
        var state = new IntersectionState(
            new NodeId("A"),
            new PhaseId("P1"),
            PhaseState.Green,
            TimeInPhaseSeconds: 5,
            QueueLengths: new Dictionary<MovementId, int> { [movement] = 3 });

        state.QueueLengths[movement].Should().Be(3);
    }

    [Fact]
    public void Ids_with_same_value_are_equal()
    {
        new MovementId("N-A-S").Should().Be(new MovementId("N-A-S"));
        new EdgeId(new NodeId("N"), new NodeId("A"))
            .Should().NotBe(new EdgeId(new NodeId("A"), new NodeId("N")));
    }
}
