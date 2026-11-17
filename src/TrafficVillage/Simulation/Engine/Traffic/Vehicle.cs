using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Engine.Traffic;


/// <inheritdoc/> 
public sealed class Vehicle : IVehicle
{
    public VehicleId Id { get; }
    public NodeId Origin { get; }
    public NodeId Destination { get; }

    public IReadOnlyList<Edge> Route { get; }

    public Edge CurrentEdge { get; private set; }

    public int RemainingEdgeTicks { get; }

    public int RouteTime { get; private set;}

    public int WaitTime { get; private set; }

    public Vehicle(VehicleId id, NodeId origin, NodeId destination, IReadOnlyList<Edge> route)
    {
        if (route.Count == 0)
        {
            throw new ArgumentException("Route cannot be empty", nameof(route));
        }

        Id = id;
        Origin = origin;
        Destination = destination;
        Route = route;
        CurrentEdge = route[0];
        RemainingEdgeTicks = CurrentEdge.Length;
    }

}

