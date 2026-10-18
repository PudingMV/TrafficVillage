using FluentAssertions;
using TrafficVillage.Controllers;
using TrafficVillage.Model;

namespace TrafficVillage.Controllers.Tests;

public class NoOpControllerTests
{
    [Fact]
    public void Decide_always_returns_keep_phase()
    {
        var controller = new NoOpController();
        var state = new NetworkState(
            0,
            Array.Empty<Vehicle>(),
            new Dictionary<NodeId, IntersectionState>());

        var action = controller.Decide(state, new NodeId("A"));

        action.Kind.Should().Be(ControllerActionKind.KeepPhase);
    }
}
