namespace TrafficVillage.Model;

public enum NodeKind
{
    Home,
    Work,
    Other,
    Intersection
}

public enum VehicleState
{
    Moving,
    Queued,
    Exited
}

public enum PhaseState
{
    Green,
    Yellow,
    AllRed
}

public enum ControllerActionKind
{
    KeepPhase,
    RequestSwitch,
    ExtendGreen
}
