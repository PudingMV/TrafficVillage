using System.Diagnostics.CodeAnalysis;
using TrafficVillage.Model;

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

    public FakeFourWay(double armLengthMeters = 100, double speedMetersPerSecond = 13.9)
    {
        List<Node> nodes =
        [
            new(A, new Vec2(0, 0), NodeKind.Intersection, HasTrafficLight: true),
            new(new NodeId("N"), new Vec2(0, armLengthMeters), NodeKind.Home, HasTrafficLight: false),
            new(new NodeId("E"), new Vec2(armLengthMeters, 0), NodeKind.Home, HasTrafficLight: false),
            new(new NodeId("S"), new Vec2(0, -armLengthMeters), NodeKind.Work, HasTrafficLight: false),
            new(new NodeId("W"), new Vec2(-armLengthMeters, 0), NodeKind.Work, HasTrafficLight: false),
        ];

        var edges = new List<Edge>();
        foreach (var arm in Arms)
        {
            edges.Add(new Edge(EdgeInto(arm), new NodeId(arm), A, armLengthMeters, speedMetersPerSecond));
            edges.Add(new Edge(EdgeOutOf(arm), A, new NodeId(arm), armLengthMeters, speedMetersPerSecond));
        }

        Movement[] movements =
        [
            Move("N", "S", TurnKind.Through), Move("N", "W", TurnKind.Right), Move("N", "E", TurnKind.Left),
            Move("S", "N", TurnKind.Through), Move("S", "E", TurnKind.Right), Move("S", "W", TurnKind.Left),
            Move("E", "W", TurnKind.Through), Move("E", "N", TurnKind.Right), Move("E", "S", TurnKind.Left),
            Move("W", "E", TurnKind.Through), Move("W", "S", TurnKind.Right), Move("W", "N", TurnKind.Left),
        ];

        PhaseDefinition[] phases =
        [
            Phase(NorthSouth, "N-A-S", "S-A-N", "N-A-W", "S-A-E"),
            Phase(EastWest, "E-A-W", "W-A-E", "E-A-N", "W-A-S"),
            Phase(NorthSouthLeft, "N-A-E", "S-A-W"),
            Phase(EastWestLeft, "E-A-S", "W-A-N"),
        ];

        _nodes = nodes.ToDictionary(n => n.Id);
        _edges = edges.ToDictionary(e => e.Id);
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

    public IReadOnlyList<NodeId> ControlledIntersectionIds { get; } = [A];

    public IntersectionDefinition Intersection { get; }

    public bool TryGetNode(NodeId id, [MaybeNullWhen(false)] out Node node) =>
        _nodes.TryGetValue(id, out node);

    public bool TryGetEdge(EdgeId id, [MaybeNullWhen(false)] out Edge edge) =>
        _edges.TryGetValue(id, out edge);

    public IReadOnlyList<Edge> IncomingEdges(NodeId nodeId) =>
        Edges.Where(e => e.To == nodeId).ToList();

    public IReadOnlyList<Edge> OutgoingEdges(NodeId nodeId) =>
        Edges.Where(e => e.From == nodeId).ToList();

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

    private static EdgeId EdgeInto(string arm) => new($"{arm}-A");

    private static EdgeId EdgeOutOf(string arm) => new($"A-{arm}");

    private static Movement Move(string from, string to, TurnKind turn) =>
        new(
            new MovementId($"{from}-A-{to}"),
            A,
            EdgeInto(from),
            EdgeOutOf(to),
            new NodeId(from),
            new NodeId(to),
            turn);

    private static PhaseDefinition Phase(PhaseId id, params string[] movementIds) =>
        new(
            id,
            movementIds.Select(m => new MovementId(m)).ToList(),
            MinGreenSeconds: 10,
            MaxGreenSeconds: 60);
}
