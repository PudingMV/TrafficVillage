namespace TrafficVillage.Simulation.Engine.RoadNetwork.Intersections;

/// <summary>
/// Position of a movement's lanes on a circle around the junction.
/// Arms are numbered clockwise; arm k has its incoming lane at 2k and its outgoing lane at 2k + 1
/// (right-hand traffic).
/// </summary>
public readonly record struct MovementDocks(int In, int Out);

public static class ConflictRules
{
    /// <summary>
    /// Two movements conflict if they merge into the same exit or their paths cross inside the junction.
    /// Movements from the same approach use separate lanes and never conflict.
    /// </summary>
    public static bool Conflicts(MovementDocks a, MovementDocks b)
    {
        if (a.Out == b.Out)
        {
            return true;
        }

        if (a.In == b.In)
        {
            return false;
        }

        return Between(b.In, a.In, a.Out) != Between(b.Out, a.In, a.Out);
    }

    private static bool Between(int x, int from, int to) =>
        from < to ? x > from && x < to : x > from || x < to;
}
