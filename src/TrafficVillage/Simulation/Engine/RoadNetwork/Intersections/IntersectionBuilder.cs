using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Engine.RoadNetwork.Intersections;

/// <summary>
/// Turns the edges around each intersection node into movements and conflict-free phases.
/// Uses only graph structure: edges plus the clockwise arm order of the node. Assumes right-hand traffic.
/// </summary>
public static class IntersectionBuilder
{
    public static Dictionary<NodeId, IntersectionDefinition> BuildAll(
        IReadOnlyList<INode> nodes,
        double minGreenSeconds = 10,
        double maxGreenSeconds = 60)
    {
        var incoming = nodes.ToDictionary(node => node.Id, _ => new List<Edge>());

        foreach (var edge in nodes.SelectMany(node => node.OutgoingEdges))
        {
            if (!incoming.TryGetValue(edge.To, out var list))
            {
                throw new InvalidOperationException($"Edge {edge.From}->{edge.To} points to an unknown node.");
            }

            list.Add(edge);
        }

        return nodes
            .Where(node => node.Kind == NodeKind.Intersection)
            .ToDictionary(
                node => node.Id,
                node => Build(node, incoming[node.Id], minGreenSeconds, maxGreenSeconds));
    }

    public static IntersectionDefinition Build(
        INode node,
        IReadOnlyList<Edge> incoming,
        double minGreenSeconds = 10,
        double maxGreenSeconds = 60)
    {
        var armIndex = ArmIndex(node, incoming);
        var movements = new List<Movement>();
        var docks = new List<MovementDocks>();

        foreach (var inEdge in incoming.OrderBy(edge => edge.From.Value, StringComparer.Ordinal))
        {
            foreach (var outEdge in node.OutgoingEdges.OrderBy(edge => edge.To.Value, StringComparer.Ordinal))
            {
                if (outEdge.To == inEdge.From)
                {
                    continue;
                }

                movements.Add(new Movement(
                    new MovementId($"{inEdge.From}-{node.Id}-{outEdge.To}"),
                    node.Id,
                    new EdgeId(inEdge.From, node.Id),
                    new EdgeId(node.Id, outEdge.To),
                    inEdge.From,
                    outEdge.To));

                if (armIndex is not null)
                {
                    docks.Add(new MovementDocks(2 * armIndex[inEdge.From], 2 * armIndex[outEdge.To] + 1));
                }
            }
        }

        var n = movements.Count;
        var conflicts = new bool[n, n];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                if (i == j)
                {
                    continue;
                }

                conflicts[i, j] = armIndex is not null
                    ? ConflictRules.Conflicts(docks[i], docks[j])
                    : movements[i].Incoming != movements[j].Incoming;
            }
        }

        var masks = PhaseGenerator.MaximalPhases(conflicts);

        var phases = masks
            .Select((mask, index) => new PhaseDefinition(
                new PhaseId($"{node.Id}:P{index + 1}"),
                Enumerable.Range(0, n)
                    .Where(m => (mask & (1UL << m)) != 0)
                    .Select(m => movements[m].Id)
                    .ToList(),
                minGreenSeconds,
                maxGreenSeconds))
            .ToList();

        var cycle = PhaseGenerator.CoveringCycle(masks, n)
            .Select(index => phases[index].Id)
            .ToList();

        return new IntersectionDefinition(node.Id, movements, phases, cycle);
    }

    /// <summary>
    /// Position of each neighbour in the clockwise order, or null when the order is unknown.
    /// </summary>
    private static Dictionary<NodeId, int>? ArmIndex(INode node, IReadOnlyList<Edge> incoming)
    {
        var neighbours = incoming.Select(edge => edge.From)
            .Concat(node.OutgoingEdges.Select(edge => edge.To))
            .ToHashSet();

        IReadOnlyList<NodeId> order = node.ArmOrder;

        if (order.Count == 0)
        {
            if (neighbours.Count > 2)
            {
                return null;
            }

            order = neighbours.OrderBy(id => id.Value, StringComparer.Ordinal).ToList(); // 2 arms: only one cyclic order exists
        }
        else if (order.Count != neighbours.Count || !neighbours.SetEquals(order))
        {
            throw new InvalidOperationException(
                $"ArmOrder of {node.Id} must list each neighbour exactly once: " +
                $"expected {{{string.Join(", ", neighbours)}}}, got [{string.Join(", ", order)}].");
        }

        return order
            .Select((id, index) => (id, index))
            .ToDictionary(pair => pair.id, pair => pair.index);
    }
}
