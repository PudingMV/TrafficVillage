namespace TrafficVillage.Model;

public sealed record ControllerRequestResult
{
    public ControllerRequestResultState State { get; }
    public PhaseState? CurrentState { get; }
    public string? Reason { get; }

    public ControllerRequestResult(ControllerRequestResultState State, PhaseState? CurrentState = null, string? Reason = null)
    {
        this.State = State;
        this.CurrentState = CurrentState;
        this.Reason = Reason;
    }

    public static ControllerRequestResult Fulfilled(PhaseState CurrentState)
    {
        return new ControllerRequestResult(ControllerRequestResultState.Fulfilled, CurrentState);
    }

    public static ControllerRequestResult AlreadyActive(PhaseState CurrentState)
    {
        return new ControllerRequestResult(ControllerRequestResultState.AlreadyActive, CurrentState);
    }

    public static ControllerRequestResult Invalid(string Reason)
    {
        return new ControllerRequestResult(ControllerRequestResultState.Invalid, null, Reason);
    }

    public static ControllerRequestResult Invalid(PhaseState CurrentState, string Reason)
    {
        return new ControllerRequestResult(ControllerRequestResultState.Invalid, CurrentState, Reason);
    }

    public static ControllerRequestResult Rejected(PhaseState CurrentState, string? Reason = null)
    {
        return new ControllerRequestResult(ControllerRequestResultState.Rejected, CurrentState, Reason);
    }
}
