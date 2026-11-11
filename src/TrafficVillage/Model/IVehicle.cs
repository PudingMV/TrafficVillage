namespace TrafficVillage.Model;


/// <summary>
/// Represents a vehicle in the simulation.
/// </summary>
public interface IVehicle
{
    VehicleId Id { get; }
    NodeId Origin { get; }
    NodeId Destination { get; }

    /// <summary>
    /// Route of the vehicle.
    /// </summary>
    IReadOnlyList<Edge> Route { get; }

    /// <summary>
    /// Current edge vehicle is on.
    /// </summary>
    Edge CurrentEdge { get; }

    /// <summary>
    /// Number of ticks remaining on the current edge.
    /// </summary>
    int RemainingEdgeTicks { get; }
}

