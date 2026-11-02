using TrafficVillage.Model;

namespace TrafficVillage.Simulation.Controllers;

/// <summary>
/// Simple timer based traffic controller. Changes phase every n-th second.
/// </summary>
public sealed class SimpleTimerController : ITrafficController
{
    private double _nextLightChange;
    private readonly double _trafficLightDuration;

    public SimpleTimerController()
    {
        _trafficLightDuration = 10.0;
        _nextLightChange = _trafficLightDuration;
    }

    public SimpleTimerController(double trafficLightDuration)
    {
        _trafficLightDuration = trafficLightDuration;
        _nextLightChange = _trafficLightDuration;
    }

    public ControllerAction Decide(NetworkState state, NodeId intersectionId)
    {
        if (state.CurrentTimeSeconds < _nextLightChange) return new ControllerAction(ControllerActionKind.KeepPhase);

        _nextLightChange += _trafficLightDuration;
        return new ControllerAction(ControllerActionKind.RequestSwitch);
    }
}
