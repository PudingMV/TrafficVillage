using FluentAssertions;
using TrafficVillage.Model;
using TrafficVillage.Simulation.Controllers;
using TrafficVillage.Tests.Fakes;

namespace TrafficVillage.Tests.Unit.Simulation.Controllers;

public class SimpleTimerControllerTests
{
    private static NetworkState StateWith(
        FakeFourWay fake,
        PhaseId phase,
        PhaseState phaseState,
        double timeInPhaseSeconds)
    {
        var junction = fake.InitialState().Intersections[FakeFourWay.A] with
        {
            CurrentPhase = phase,
            PhaseState = phaseState,
            TimeInPhaseSeconds = timeInPhaseSeconds
        };

        return new NetworkState(
            timeInPhaseSeconds,
            [],
            new Dictionary<NodeId, IntersectionState> { [FakeFourWay.A] = junction });
    }

    [Fact]
    public void Decide_holds_current_phase_before_duration_elapses()
    {
        var fake = new FakeFourWay();
        var controller = new SimpleTimerController(fake, trafficLightDuration: 10.0);
        var state = StateWith(fake, FakeFourWay.NorthSouth, PhaseState.Green, timeInPhaseSeconds: 9.9);

        var request = controller.Decide(state, FakeFourWay.A);

        request.DesiredPhase.Should().Be(FakeFourWay.NorthSouth);
    }

    [Fact]
    public void Decide_advances_to_next_phase_when_duration_elapses()
    {
        var fake = new FakeFourWay();
        var controller = new SimpleTimerController(fake, trafficLightDuration: 10.0);
        var state = StateWith(fake, FakeFourWay.NorthSouth, PhaseState.Green, timeInPhaseSeconds: 10.0);

        var request = controller.Decide(state, FakeFourWay.A);

        request.DesiredPhase.Should().Be(FakeFourWay.EastWest);
    }

    [Fact]
    public void Decide_wraps_around_to_first_phase_after_last_phase_in_cycle()
    {
        var fake = new FakeFourWay();
        var controller = new SimpleTimerController(fake, trafficLightDuration: 10.0);
        var state = StateWith(fake, FakeFourWay.EastWestLeft, PhaseState.Green, timeInPhaseSeconds: 10.0);

        var request = controller.Decide(state, FakeFourWay.A);

        request.DesiredPhase.Should().Be(FakeFourWay.NorthSouth);
    }

    [Fact]
    public void Decide_holds_current_phase_when_signal_is_not_green()
    {
        var fake = new FakeFourWay();
        var controller = new SimpleTimerController(fake, trafficLightDuration: 10.0);
        var state = StateWith(fake, FakeFourWay.NorthSouth, PhaseState.Yellow, timeInPhaseSeconds: 15.0);

        var request = controller.Decide(state, FakeFourWay.A);

        request.DesiredPhase.Should().Be(FakeFourWay.NorthSouth);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-5.0)]
    public void Constructor_throws_when_duration_is_not_positive(double duration)
    {
        var fake = new FakeFourWay();

        var act = () => new SimpleTimerController(fake, duration);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
