using FluentAssertions;
using TrafficVillage.Model;
using TrafficVillage.Simulation.Engine.RoadNetwork;
using TrafficVillage.Tests.Fakes;

namespace TrafficVillage.Tests.Unit.Simulation.Engine.Intersections;

public class IntersectionBuilderTests
{
    private static readonly NodeId A = new("A");

    /// <param name="arms">Listed clockwise; used as ArmOrder when <paramref name="withOrder"/> is true.</param>
    private static IntersectionDefinition Junction(bool withOrder, params (string Id, bool In, bool Out)[] arms)
    {
        var nodes = new List<Node>();
        var aOutgoing = new List<Edge>();

        foreach (var arm in arms)
        {
            var id = new NodeId(arm.Id);
            List<Edge> outgoing = arm.In ? [new Edge(id, A, 10)] : [];
            if (arm.Out)
            {
                aOutgoing.Add(new Edge(A, id, 10));
            }

            nodes.Add(new Node(id, NodeKind.Entry, outgoing));
        }

        var order = withOrder ? arms.Select(arm => new NodeId(arm.Id)).ToList() : null;
        nodes.Add(new Node(A, NodeKind.Intersection, aOutgoing, order));

        RoadNetwork.FromNodes(nodes).TryGetIntersection(A, out var intersection).Should().BeTrue();
        return intersection!;
    }

    private static IntersectionDefinition FourWay() => Junction(
        true, ("N", true, true), ("E", true, true), ("S", true, true), ("W", true, true));

    private static bool Together(IntersectionDefinition def, params string[] movementIds) =>
        def.Phases.Any(phase => movementIds.All(id => phase.GreenMovements.Contains(new MovementId(id))));

    [Fact]
    public void Four_way_has_twelve_movements() =>
        FourWay().Movements.Should().HaveCount(12);

    [Fact]
    public void Opposite_throughs_share_a_phase() =>
        Together(FourWay(), "N-A-S", "S-A-N").Should().BeTrue();

    [Fact]
    public void Perpendicular_throughs_never_share_a_phase() =>
        Together(FourWay(), "N-A-S", "E-A-W").Should().BeFalse();

    [Fact]
    public void Opposing_lefts_share_a_phase() =>
        Together(FourWay(), "N-A-E", "S-A-W").Should().BeTrue();

    [Fact]
    public void Movements_into_the_same_exit_never_share_a_phase() =>
        Together(FourWay(), "N-A-S", "W-A-S").Should().BeFalse();

    [Fact]
    public void Whole_approach_can_go_together() =>
        Together(FourWay(), "N-A-S", "N-A-W", "N-A-E").Should().BeTrue();

    [Fact]
    public void Default_cycle_gives_every_movement_green()
    {
        var def = FourWay();
        var greened = def.DefaultCycle
            .SelectMany(id => def.Phases.Single(p => p.Id == id).GreenMovements)
            .ToHashSet();

        greened.Should().BeEquivalentTo(def.Movements.Select(m => m.Id));
    }

    [Fact]
    public void Hand_written_fake_phases_are_inside_generated_phases()
    {
        var generated = FourWay();

        foreach (var handPhase in new FakeFourWay().Intersection.Phases)
        {
            Together(generated, handPhase.GreenMovements.Select(m => m.Value).ToArray())
                .Should().BeTrue($"{handPhase.Id} must be conflict-free");
        }
    }

    [Fact]
    public void Generation_is_deterministic()
    {
        var first = FourWay();
        var second = FourWay();

        second.Phases.Select(p => (p.Id, string.Join(",", p.GreenMovements)))
            .Should().Equal(first.Phases.Select(p => (p.Id, string.Join(",", p.GreenMovements))));
    }

    [Fact]
    public void T_junction_has_six_movements_and_full_cover()
    {
        var def = Junction(true, ("N", true, true), ("E", true, true), ("W", true, true));

        def.Movements.Should().HaveCount(6);
        def.DefaultCycle.Should().NotBeEmpty();
    }

    [Fact]
    public void One_way_arm_contributes_only_incoming_movements() =>
        Junction(true, ("N", true, true), ("E", true, false), ("S", true, true), ("W", true, true))
            .Movements.Should().HaveCount(9);

    [Fact]
    public void Without_arm_order_each_approach_gets_its_own_phase()
    {
        var def = Junction(false, ("N", true, true), ("E", true, true), ("S", true, true), ("W", true, true));

        def.Phases.Should().HaveCount(4);
        def.Phases.Should().AllSatisfy(phase =>
            phase.GreenMovements
                .Select(id => def.Movements.Single(m => m.Id == id).Incoming)
                .Distinct()
                .Should().ContainSingle());
    }

    [Fact]
    public void Two_arm_node_needs_no_order_and_lets_both_directions_go()
    {
        var def = Junction(false, ("N", true, true), ("S", true, true));

        def.Phases.Should().ContainSingle()
            .Which.GreenMovements.Should().HaveCount(2);
    }

    [Fact]
    public void Arm_order_that_does_not_match_neighbours_is_rejected()
    {
        var nodes = new List<Node>
        {
            new(new NodeId("N"), NodeKind.Entry, [new Edge(new NodeId("N"), A, 10)]),
            new(new NodeId("S"), NodeKind.Entry, [new Edge(new NodeId("S"), A, 10)]),
            new(A, NodeKind.Intersection, [], [new NodeId("N"), new NodeId("X")]),
        };

        var act = () => RoadNetwork.FromNodes(nodes);

        act.Should().Throw<InvalidOperationException>();
    }
}
