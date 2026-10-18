# Architecture

## Overview

TrafficVillage is a village-scale traffic simulator for comparing traffic light control algorithms.
It is **simulation software**, not a deployable real-world traffic control system.

## High-level diagram

```
CLI/UI
  ↓
AppOrchestrator (Marek)
  ├── loads map + demand
  ├── wires controllers + metrics
  └── calls SimulationManager
        ↓
SimulationManager (Michał)
  ├── reads RoadNetwork (Eryk)
  ├── moves vehicles
  └── asks Controllers (Bartek)
        ↓
Metrics (Marek) — observer only
```

## Solution structure

```
TrafficVillage.sln
├── src/
│   ├── TrafficVillage.Model/         ← shared types + interfaces
│   ├── TrafficVillage.Utils/         ← helpers
│   ├── TrafficVillage.Network/       ← graph, routing, map loading
│   ├── TrafficVillage.Simulation/      ← engine, vehicles, phases
│   ├── TrafficVillage.Controllers/     ← algorithms
│   ├── TrafficVillage.Metrics/         ← stats, experiments
│   └── TrafficVillage.Cli/             ← entry point
└── tests/
    ├── TrafficVillage.*.Tests/         ← unit tests per module
    └── TrafficVillage.Integration.Tests/
```

## Design principles

1. **Orchestrator pattern** — `SimulationManager` coordinates; modules don't call each other freely
2. **Immutable snapshots** — controllers receive `NetworkState`, never mutate it
3. **Pluggable algorithms** — `ITrafficController` implementations are swappable
4. **CLI-first** — headless simulation is the primary deliverable; UI optional (week 11+)
5. **Reproducibility** — seeded RNG, deterministic tick loop

## Tiered delivery

1. **Weeks 1–2:** Contracts, repo, CI, stub implementations
2. **Weeks 3–4:** Network + routing; load map, compute routes
3. **Weeks 5–6:** Simulation engine v1 (1 intersection)
4. **Weeks 7–8:** Fixed + adaptive controllers, metrics, CLI comparison
5. **Weeks 9–10:** Village map (6–10 nodes), multi-intersection
6. **Weeks 11–12:** Advanced algorithm (green wave or genetic)
7. **Weeks 13–15:** Optional UI, golden tests, report
