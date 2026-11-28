using FluentAssertions;
using TrafficVillage.Model;
using TrafficVillage.Simulation.Controllers;
using TrafficVillage.Tests.Fakes;

namespace TrafficVillage.Tests.Unit.Simulation.Controllers;

public class NoOpControllerTests
{
    [Fact]
    public void Decide_returns_current_phase_without_changing_it()
    {
        var fake = new FakeFourWay();
        var controller = new NoOpController();
        var junction = fake.InitialState().Intersections[FakeFourWay.A] with
        {
            CurrentPhase = FakeFourWay.EastWest,
            PhaseState = PhaseState.Green,
            TimeInPhaseSeconds = 99.0
        };
        var state = new NetworkState(
            99.0,
            [],
            new Dictionary<NodeId, IntersectionState> { [FakeFourWay.A] = junction });

        var request = controller.Decide(state, FakeFourWay.A);

        request.DesiredPhase.Should().Be(FakeFourWay.EastWest);
    }

    [Fact]
    public void Decide_throws_when_intersection_is_missing_from_state()
    {
        var controller = new NoOpController();
        var state = new NetworkState(0, [], new Dictionary<NodeId, IntersectionState>());

        var act = () => controller.Decide(state, new NodeId("UNKNOWN"));

        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void Decide_throws_when_state_is_null()
    {
        var controller = new NoOpController();

        var act = () => controller.Decide(null!, FakeFourWay.A);

        act.Should().Throw<ArgumentNullException>();
    }
}
