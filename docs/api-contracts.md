# API Contracts

This document defines the shared interfaces and types in `TrafficVillage.Model`.
**Changes to this module require a PR review from at least one other team member.**

## Module ownership

| Module | Owner | Responsibility |
|--------|-------|----------------|
| `TrafficVillage.Model` | Szymon & Kądź | Types, interfaces, enums |
| `TrafficVillage.Utils` | Szymon & Kądź | Pure helpers (parsers, guards) |
| `TrafficVillage.Network` | Eryk | Graph, map loading, routing |
| `TrafficVillage.Simulation` | Michał | Tick loop, vehicles, phases |
| `TrafficVillage.Controllers` | Bartek | Traffic light algorithms |
| `TrafficVillage.Metrics` | Marek | Stats, experiment runner |
| `TrafficVillage.Cli` | Marek | AppOrchestrator, CLI entry point |
| Integration tests | Szymon | End-to-end wiring tests |

## Dependency rules

```
Cli → Simulation, Metrics, Network, Controllers, Model
Simulation → Network, Controllers, Model
Controllers → Model
Network → Model, Utils
Metrics → Model
Utils → Model
Model → (nothing)
```

**Forbidden:** Controllers → Simulation, Metrics → Simulation (during ticks).

## Core interfaces

### ITrafficController

```csharp
public interface ITrafficController
{
    ControllerAction Decide(NetworkState state, NodeId intersectionId);
}
```

- Receives **read-only** `NetworkState` snapshots
- Returns `KeepPhase`, `RequestSwitch`, or `ExtendGreen`
- Never mutates vehicles or intersection state directly

### IMetricsCollector

```csharp
public interface IMetricsCollector
{
    void OnTick(NetworkState state);
    void OnVehicleExited(Vehicle vehicle);
    SimulationMetrics Finalize();
}
```

- Observes simulation; **never controls** it
- Called once per tick by `SimulationManager`

### ISimulationManager

```csharp
public interface ISimulationManager
{
    SimulationResult Run(
        IRoadNetwork network,
        IReadOnlyDictionary<NodeId, ITrafficController> controllers,
        IMetricsCollector metrics,
        int seed,
        double tickDurationSeconds,
        double durationSeconds);
}
```

Single integration anchor for the simulation engine.

### IRoadNetwork / IRoutePlanner

```csharp
public interface IRoadNetwork
{
    IReadOnlyList<NodeId> NodeIds { get; }
    bool TryGetNodeKind(NodeId nodeId, out NodeKind kind);
}

public interface IRoutePlanner
{
    IReadOnlyList<EdgeId> FindRoute(NodeId from, NodeId to);
}
```

## Immutable state records

| Type | Purpose |
|------|---------|
| `NetworkState` | Full snapshot at a point in time |
| `IntersectionState` | Phase, queues per approach |
| `Vehicle` | Route, position, wait time, state |
| `ControllerAction` | What a controller wants to do |
| `SimulationResult` | Final output of a run |
| `SimulationMetrics` | Average wait, P95, throughput, etc. |

## Per-tick flow (SimulationManager)

1. Spawn vehicles from demand
2. Build read-only `NetworkState` snapshot
3. For each intersection: `controller.Decide(state, intersectionId)`
4. Safety layer validates action
5. Apply phase transitions (Green → Yellow → AllRed → Green)
6. Move / queue vehicles
7. `metrics.OnTick(state)`
8. Repeat until duration reached

## Phase state machine (mandatory)

```
Green → Yellow → AllRed → Green
```

**Invariants:**
- Conflicting approaches never green simultaneously
- `minGreen` / `maxGreen` always respected

## Metrics (primary)

| Metric | Description |
|--------|-------------|
| Average wait time | Primary comparison metric |
| P95 wait time | Tail latency |
| Max queue length per approach | Congestion indicator |
| Throughput | Vehicles exited per hour |
| Stop count per vehicle | Quality of flow |

## Reproducibility

Same `(map, demand, controller, seed)` → same results.
Golden scenario tests will assert expected metric ranges.
