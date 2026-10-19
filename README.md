# TrafficVillage

Village-scale traffic light simulation for comparing control algorithms.
University team project — simulation software, not a real-world deployable system.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Quick start

```bash
# Build
dotnet build

# Run tests
dotnet test

# Run simulation
dotnet run --project src/TrafficVillage.Cli
```

## Project structure

| Module | Owner | Description |
|--------|-------|-------------|
| `TrafficVillage.Model` | Szymon & Kądź | Shared types and interfaces |
| `TrafficVillage.Utils` | Szymon & Kądź | Helpers and parsers |
| `TrafficVillage.Network` | Eryk | Graph, map loading, routing |
| `TrafficVillage.Simulation` | Michał | Simulation engine and tick loop |
| `TrafficVillage.Controllers` | Bartek | Traffic light algorithms |
| `TrafficVillage.Metrics` | Marek | Metrics and experiments |
| `TrafficVillage.Cli` | Marek | CLI entry point and orchestrator |

See [docs/api-contracts.md](docs/api-contracts.md) for interface definitions.
See [docs/architecture.md](docs/architecture.md) for design overview.

## Git workflow

1. **Never push directly to `main`**
2. Create a feature branch: `feature/<short-description>`
3. Open a Pull Request
4. Get **1 approval** from a teammate (not the PR author)
5. CI must pass before merge

## Resources

- `resources/maps/` — JSON map files (e.g. `single_intersection.json`, `village_01.json`)
- `resources/demand/` — YAML demand scenarios (e.g. `morning_rush.yaml`)

## CI

GitHub Actions runs `dotnet build` and `dotnet test` on every push and PR to `main`.
