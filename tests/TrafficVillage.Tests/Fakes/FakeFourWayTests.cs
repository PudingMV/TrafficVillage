using FluentAssertions;
using TrafficVillage.Model;

namespace TrafficVillage.Tests.Fakes;

public class FakeFourWayTests
{
    private readonly FakeFourWay _network = new();

    [Fact]
    public void Junction_has_twelve_movements()
    {
        _network.Intersection.Movements.Should().HaveCount(12);
    }

    [Fact]
    public void Every_movement_is_green_in_some_phase()
    {
        var green = _network.Intersection.Phases.SelectMany(p => p.GreenMovements);

        _network.Intersection.Movements.Select(m => m.Id).Should().BeSubsetOf(green);
    }

    [Fact]
    public void Movements_only_use_existing_edges()
    {
        foreach (var movement in _network.Intersection.Movements)
        {
            _network.GetEdgeByRelatedNodeIds(movement.Incoming.SourceNodeId, movement.Incoming.DestinationNodeId).Should().NotBeNull();
            _network.GetEdgeByRelatedNodeIds(movement.Outgoing.SourceNodeId, movement.Outgoing.DestinationNodeId).Should().NotBeNull();
        }
    }

    [Fact]
    public void Route_from_north_to_south_passes_through_the_junction()
    {
        var route = _network.FindRoute(new NodeId("N"), new NodeId("S"));

        route.Should().Equal(
            new EdgeId(new NodeId("N"), FakeFourWay.A),
            new EdgeId(FakeFourWay.A, new NodeId("S")));
    }

    [Fact]
    public void Initial_state_has_a_queue_for_every_movement()
    {
        var state = _network.InitialState();

        state.Intersections[FakeFourWay.A].QueueLengths.Keys
            .Should().BeEquivalentTo(_network.Intersection.Movements.Select(m => m.Id));
    }
}
