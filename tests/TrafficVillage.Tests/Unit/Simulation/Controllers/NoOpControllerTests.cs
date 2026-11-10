using FluentAssertions;
using TrafficVillage.Model;
using TrafficVillage.Simulation.Controllers;

namespace TrafficVillage.Tests.Unit.Simulation.Controllers;

public class NoOpControllerTests
{
    [Fact]
    public void Decide_always_returns_keep_phase()
    {
        var controller = new NoOpController();
        var state = new NetworkState(
            0,
            [],
            new Dictionary<NodeId, IntersectionState>());

        var action = controller.Decide(state, new NodeId("A"));

        action.Kind.Should().Be(ControllerActionKind.KeepPhase);
    }
}
