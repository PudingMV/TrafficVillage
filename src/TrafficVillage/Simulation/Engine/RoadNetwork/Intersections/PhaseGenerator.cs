using System.Numerics;

namespace TrafficVillage.Simulation.Engine.RoadNetwork.Intersections;

/// <summary>
/// Finds conflict-free movement sets. A phase is a bitmask over movement indices.
/// </summary>
public static class PhaseGenerator
{
    /// <summary>
    /// All maximal sets of mutually compatible movements, sorted by mask for reproducible ids.
    /// </summary>
    public static IReadOnlyList<ulong> MaximalPhases(bool[,] conflicts)
    {
        var n = conflicts.GetLength(0);
        if (n > 64)
        {
            throw new ArgumentException($"At most 64 movements per intersection are supported, got {n}.", nameof(conflicts));
        }

        var compatible = new ulong[n];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                if (i != j && !conflicts[i, j])
                {
                    compatible[i] |= 1UL << j;
                }
            }
        }

        var result = new List<ulong>();
        if (n > 0)
        {
            Expand(0, AllBits(n), 0, compatible, result);
        }

        result.Sort();
        return result;
    }

    /// <summary>
    /// Greedy cover: repeatedly picks the phase that greens the most still-uncovered movements.
    /// Returns indices into <paramref name="phases"/>.
    /// </summary>
    public static IReadOnlyList<int> CoveringCycle(IReadOnlyList<ulong> phases, int movementCount)
    {
        var uncovered = AllBits(movementCount);
        var cycle = new List<int>();

        while (uncovered != 0)
        {
            var best = -1;
            var bestGain = 0;
            for (var i = 0; i < phases.Count; i++)
            {
                var gain = BitOperations.PopCount(phases[i] & uncovered);
                if (gain > bestGain)
                {
                    best = i;
                    bestGain = gain;
                }
            }

            if (best < 0)
            {
                throw new InvalidOperationException("Some movements are not part of any phase.");
            }

            cycle.Add(best);
            uncovered &= ~phases[best];
        }

        return cycle;
    }

    // Bron–Kerbosch with pivoting on the compatibility graph: R = current set, P = candidates, X = excluded.
    private static void Expand(ulong r, ulong p, ulong x, ulong[] compatible, List<ulong> result)
    {
        if (p == 0 && x == 0)
        {
            result.Add(r);
            return;
        }

        var pivot = BitOperations.TrailingZeroCount(p | x);
        var candidates = p & ~compatible[pivot];

        while (candidates != 0)
        {
            var v = BitOperations.TrailingZeroCount(candidates);
            var bit = 1UL << v;

            Expand(r | bit, p & compatible[v], x & compatible[v], compatible, result);

            p &= ~bit;
            x |= bit;
            candidates &= ~bit;
        }
    }

    private static ulong AllBits(int n) => n == 64 ? ulong.MaxValue : (1UL << n) - 1;
}
