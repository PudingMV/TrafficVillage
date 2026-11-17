namespace TrafficVillage.Model;

/// <summary>
/// Observes simulation state each tick. Never controls the simulation.
/// </summary>
public interface IMetricsCollector
{
    void OnTick(NetworkState state);

    void OnVehicleExited(IVehicle vehicle);

    SimulationMetrics Finalize();
}
