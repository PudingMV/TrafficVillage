using FluentAssertions;
using TrafficVillage.Model;

namespace TrafficVillage.Model.Tests;

public class ModelContractsTests
{
    [Fact]
    public void ControllerAction_defaults_to_keep_phase()
    {
        var action = new ControllerAction(ControllerActionKind.KeepPhase);

        action.Kind.Should().Be(ControllerActionKind.KeepPhase);
        action.TargetPhase.Should().BeNull();
    }

    [Fact]
    public void NetworkState_can_be_created_with_empty_collections()
    {
        var state = new NetworkState(
            0,
            Array.Empty<Vehicle>(),
            new Dictionary<NodeId, IntersectionState>());

        state.CurrentTimeSeconds.Should().Be(0);
        state.Vehicles.Should().BeEmpty();
        state.Intersections.Should().BeEmpty();
    }
}
