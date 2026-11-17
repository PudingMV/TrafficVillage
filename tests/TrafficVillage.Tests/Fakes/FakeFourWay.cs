using System.Diagnostics.CodeAnalysis;
using TrafficVillage.Model;
using TrafficVillage.Simulation.Engine.RoadNetwork;

namespace TrafficVillage.Tests.Fakes;

/// <summary>
/// Single four-way junction A with arms N, E, S, W. Two-way roads, right-hand traffic,
/// one queue per movement. Phases are hand-written until RoadNetwork generates them.
/// </summary>
public sealed class FakeFourWay : IRoadNetwork, IRoutePlanner
{
    public static readonly NodeId A = new("A");
    public static readonly PhaseId NorthSouth = new("P1");
    public static readonly PhaseId EastWest = new("P2");
    public static readonly PhaseId NorthSouthLeft = new("P3");
    public static readonly PhaseId EastWestLeft = new("P4");

    private static readonly string[] Arms = ["N", "E", "S", "W"];

    private readonly Dictionary<NodeId, Node> _nodes;
    private readonly Dictionary<EdgeId, Edge> _edges;

    public FakeFourWay(int edgeLengthTicks = 10)
    {
        var edges = new List<Edge>();
        var aOutgoing = new List<Edge>();
        var nodes = new List<Node>();

        foreach (var arm in Arms)
        {
            var armId = new NodeId(arm);
            var into = new Edge(armId, A, edgeLengthTicks);
            var outOf = new Edge(A, armId, edgeLengthTicks);
            edges.Add(into);
            edges.Add(outOf);
            aOutgoing.Add(outOf);
            nodes.Add(new Node(armId, NodeKind.Entry, [into]));
        }

        nodes.Insert(0, new Node(A, NodeKind.Intersection, aOutgoing));

        Movement[] movements =
        [
            Move("N", "S"), Move("N", "W"), Move("N", "E"),
            Move("S", "N"), Move("S", "E"), Move("S", "W"),
            Move("E", "W"), Move("E", "N"), Move("E", "S"),
            Move("W", "E"), Move("W", "S"), Move("W", "N"),
        ];

        PhaseDefinition[] phases =
        [
            Phase(NorthSouth, "N-A-S", "S-A-N", "N-A-W", "S-A-E"),
            Phase(EastWest, "E-A-W", "W-A-E", "E-A-N", "W-A-S"),
            Phase(NorthSouthLeft, "N-A-E", "S-A-W"),
            Phase(EastWestLeft, "E-A-S", "W-A-N"),
        ];

        _nodes = nodes.ToDictionary(n => n.Id);
        _edges = edges.ToDictionary(e => new EdgeId(e.From, e.To));
        Nodes = nodes;
        Edges = edges;
        Intersection = new IntersectionDefinition(A, movements, phases);
    }

    public static DemandProfile MorningRush { get; } = new(
    [
        new OdPair(new NodeId("N"), new NodeId("S"), 120),
        new OdPair(new NodeId("E"), new NodeId("W"), 80),
        new OdPair(new NodeId("N"), new NodeId("W"), 40),
        new OdPair(new NodeId("E"), new NodeId("S"), 30),
    ]);

    public IReadOnlyList<Node> Nodes { get; }

    public IReadOnlyList<Edge> Edges { get; }

    public IntersectionDefinition Intersection { get; }

    public INode? GetNodeById(NodeId id) => _nodes.GetValueOrDefault(id);

    public Edge? GetEdgeByRelatedNodeIds(NodeId source, NodeId destination) =>
        _edges.GetValueOrDefault(new EdgeId(source, destination));

    public bool TryGetIntersection(NodeId id, [MaybeNullWhen(false)] out IntersectionDefinition intersection)
    {
        intersection = id == A ? Intersection : null;
        return intersection is not null;
    }

    public IReadOnlyList<EdgeId> FindRoute(NodeId origin, NodeId destination)
    {
        if (origin == destination || origin == A || destination == A)
        {
            return [];
        }

        var into = EdgeInto(origin.Value);
        var outOf = EdgeOutOf(destination.Value);
        if (!_edges.ContainsKey(into) || !_edges.ContainsKey(outOf))
        {
            return [];
        }

        return [into, outOf];
    }

    public NetworkState InitialState(double timeSeconds = 0)
    {
        var queues = Intersection.Movements.ToDictionary(m => m.Id, _ => 0);
        var junction = new IntersectionState(A, NorthSouth, PhaseState.Green, 0, queues);

        return new NetworkState(
            timeSeconds,
            [],
            new Dictionary<NodeId, IntersectionState> { [A] = junction });
    }

    private static EdgeId EdgeInto(string arm) => new(new NodeId(arm), A);

    private static EdgeId EdgeOutOf(string arm) => new(A, new NodeId(arm));

    private static Movement Move(string from, string to) =>
        new(
            new MovementId($"{from}-A-{to}"),
            A,
            EdgeInto(from),
            EdgeOutOf(to),
            new NodeId(from),
            new NodeId(to));

    private static PhaseDefinition Phase(PhaseId id, params string[] movementIds) =>
        new(
            id,
            movementIds.Select(m => new MovementId(m)).ToList(),
            MinGreenSeconds: 10,
            MaxGreenSeconds: 60);
}
