namespace TrafficVillage.Model;

public readonly record struct NodeId(string Value)
{
    public override string ToString() => Value;
}

public readonly record struct EdgeId(string Value)
{
    public override string ToString() => Value;
}

public readonly record struct PhaseId(string Value)
{
    public override string ToString() => Value;
}
