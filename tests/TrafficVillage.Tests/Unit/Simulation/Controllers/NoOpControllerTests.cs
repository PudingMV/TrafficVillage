using FluentAssertions;
using TrafficVillage.Model;
using TrafficVillage.Simulation.Controllers;

namespace TrafficVillage.Tests.Unit.Simulation.Controllers;

public class NoOpControllerTests
{
    [Fact]
    public void Decide_throws_not_implemented_for_stub()
    {
        var controller = new NoOpController();
        var state = new NetworkState(
            0,
            [],
            new Dictionary<NodeId, IntersectionState>());

        // shortcut: stub throws NotImplementedException, update when NoOpController returns a real ControllerRequest or null
        var act = () => controller.Decide(state, new NodeId("A"));

        act.Should().Throw<NotImplementedException>();
    }
}
