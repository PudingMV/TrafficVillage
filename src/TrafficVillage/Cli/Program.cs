using TrafficVillage.Core;

var result = AppOrchestrator.CreateDefault().Run();

Console.WriteLine("TrafficVillage — simulation complete");
Console.WriteLine($"  Duration: {result.DurationSeconds}s");
Console.WriteLine($"  Seed:     {result.Seed}");
Console.WriteLine($"  Exited:   {result.Metrics.TotalVehiclesExited} vehicles");
