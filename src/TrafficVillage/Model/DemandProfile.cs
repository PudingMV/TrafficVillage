namespace TrafficVillage.Model;

public sealed record OdPair(
    NodeId From,
    NodeId To,
    double RatePerHour);

public sealed record DemandProfile(IReadOnlyList<OdPair> Pairs)
{
    public static DemandProfile Empty { get; } = new([]);
}
