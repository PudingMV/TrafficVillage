using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Controllers;

/// <summary>
/// Simple timer based traffic controller. Changes phase every <c>trafficLightDuration</c> seconds.
/// </summary>
// Potential ways to update:
// - implement handling of recurring Phases like [P1, P2, P1, P3]
public sealed class SimpleTimerController : ITrafficController
{
    private readonly double _trafficLightDuration;
    private readonly Dictionary<NodeId, IReadOnlyList<PhaseId>> _cyclesByIntersection;

    public SimpleTimerController(IntersectionDefinition intersection, double trafficLightDuration = 10.0)
        : this([intersection], trafficLightDuration) { }

    public SimpleTimerController(IRoadNetwork network, double trafficLightDuration = 10.0)
        : this(
            (network ?? throw new ArgumentNullException(nameof(network))).Intersections,
            trafficLightDuration)
    { }

    public SimpleTimerController(
        IEnumerable<IntersectionDefinition> intersections,
        double trafficLightDuration = 10.0)
    {
        ArgumentNullException.ThrowIfNull(intersections);

        if (trafficLightDuration <= 0 ||
            double.IsNaN(trafficLightDuration) ||
            double.IsInfinity(trafficLightDuration))
        {
            throw new ArgumentOutOfRangeException(
                nameof(trafficLightDuration),
                "Duration must be a finite positive number.");
        }

        _trafficLightDuration = trafficLightDuration;
        _cyclesByIntersection = new Dictionary<NodeId, IReadOnlyList<PhaseId>>();

        // Checks if intersection is well-defined and cache phase cycle
        foreach (var intersection in intersections)
        {
            ArgumentNullException.ThrowIfNull(intersection);

            var legalPhaseIds = intersection.Phases.Select(p => p.Id).ToHashSet();

            // When no DefaultCycle, uses given order
            var cycle = intersection.DefaultCycle.Count > 0
                ? intersection.DefaultCycle.ToArray()
                : intersection.Phases.Select(p => p.Id).ToArray();

            if (cycle.Length == 0)
            {
                throw new ArgumentException(
                    $"Intersection '{intersection.Id}' must define at least one legal phase.",
                    nameof(intersections));
            }

            if (cycle.Any(phaseId => !legalPhaseIds.Contains(phaseId)))
            {
                throw new ArgumentException(
                    $"Intersection '{intersection.Id}' contains a cycle phase not listed in Phases.",
                    nameof(intersections));
            }

            _cyclesByIntersection[intersection.Id] = cycle;
        }
    }

    public ControllerRequest Decide(NetworkState state, NodeId intersectionId)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (!state.Intersections.TryGetValue(intersectionId, out var intersection))
        {
            throw new KeyNotFoundException(
                $"Intersection '{intersectionId}' was not found in the current network state.");
        }

        if (!_cyclesByIntersection.TryGetValue(intersectionId, out var cycle))
        {
            throw new InvalidOperationException(
                $"No phase cycle was configured for intersection '{intersectionId}'.");
        }

        // Hold the active phase when not Green or until duration elapses.
        if (intersection.PhaseState != PhaseState.Green ||
            intersection.TimeInPhaseSeconds < _trafficLightDuration)
        {
            return new ControllerRequest(intersection.CurrentPhase);
        }

        var currentIndex = IndexOf(cycle, intersection.CurrentPhase);
        var nextIndex = currentIndex >= 0 ? (currentIndex + 1) % cycle.Count : 0;

        return new ControllerRequest(cycle[nextIndex]);
    }

    /// <summary>
    /// Finds the first index of <paramref name="phaseId"/> in <paramref name="cycle"/>.
    /// Assumes each phase appears at most once.
    /// </summary>
    private static int IndexOf(IReadOnlyList<PhaseId> cycle, PhaseId phaseId)
    {
        for (var i = 0; i < cycle.Count; i++)
        {
            if (cycle[i] == phaseId)
            {
                return i;
            }
        }

        return -1;
    }
}
