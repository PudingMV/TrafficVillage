using FluentAssertions;
using TrafficVillage.Simulation.Engine.RoadNetwork.Intersections;

namespace TrafficVillage.Tests.Unit.Simulation.Engine.Intersections;

/// <summary>
/// Four-way junction numbered clockwise from north: N in/out = 0/1, E = 2/3, S = 4/5, W = 6/7.
/// </summary>
public class ConflictRulesTests
{
    private static readonly MovementDocks NorthToSouth = new(0, 5);
    private static readonly MovementDocks SouthToNorth = new(4, 1);
    private static readonly MovementDocks EastToWest = new(2, 7);
    private static readonly MovementDocks NorthToWest = new(0, 7);
    private static readonly MovementDocks NorthToEast = new(0, 3);
    private static readonly MovementDocks SouthToWest = new(4, 7);
    private static readonly MovementDocks EastToSouth = new(2, 5);
    private static readonly MovementDocks WestToEast = new(6, 3);

    [Fact]
    public void Opposite_throughs_are_compatible() =>
        ConflictRules.Conflicts(NorthToSouth, SouthToNorth).Should().BeFalse();

    [Fact]
    public void Perpendicular_throughs_cross() =>
        ConflictRules.Conflicts(NorthToSouth, EastToWest).Should().BeTrue();

    [Fact]
    public void Opposing_lefts_are_compatible() =>
        ConflictRules.Conflicts(NorthToEast, SouthToWest).Should().BeFalse();

    [Fact]
    public void Left_turn_crosses_opposing_through() =>
        ConflictRules.Conflicts(NorthToEast, SouthToNorth).Should().BeTrue();

    [Fact]
    public void Same_exit_is_a_merge_conflict() =>
        ConflictRules.Conflicts(EastToWest, NorthToWest).Should().BeTrue();

    [Fact]
    public void Same_approach_is_compatible() =>
        ConflictRules.Conflicts(NorthToSouth, NorthToEast).Should().BeFalse();

    [Fact]
    public void Right_turn_never_crosses()
    {
        MovementDocks[] others = [NorthToSouth, SouthToNorth, NorthToEast, EastToSouth, WestToEast];

        others.Should().AllSatisfy(other => ConflictRules.Conflicts(NorthToWest, other).Should().BeFalse());
    }
}
