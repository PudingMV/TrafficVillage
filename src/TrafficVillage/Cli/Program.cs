using TrafficVillage.Core;

var orchestrator = new AppOrchestrator();
var result = orchestrator.RunDefaultScenario();

Console.WriteLine("TrafficVillage — simulation complete");
Console.WriteLine($"  Duration: {result.DurationSeconds}s");
Console.WriteLine($"  Seed:     {result.Seed}");
Console.WriteLine($"  Exited:   {result.Metrics.TotalVehiclesExited} vehicles");
