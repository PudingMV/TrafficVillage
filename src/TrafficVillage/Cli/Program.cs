using System.Globalization;
using TrafficVillage.Core;
using TrafficVillage.Model;

if (args.Contains("--help") || args.Contains("-h"))
{
    PrintHelp();
    return 0;
}

try
{
    CliOptions options = ParseArguments(args);

    Console.WriteLine("TrafficVillage - starting simulation...");
    Console.WriteLine($"  Map:        {options.MapPath ?? "(default)"}");
    Console.WriteLine($"  Demand:     {options.DemandPath ?? "(default)"}");
    Console.WriteLine($"  Controller: {options.ControllerName}");
    Console.WriteLine($"  Seed:       {options.Seed}");
    Console.WriteLine($"  Duration:   {options.DurationSeconds.ToString("F1", CultureInfo.InvariantCulture)}s " +
    $"(tick: {options.TickDurationSeconds.ToString("F2", CultureInfo.InvariantCulture)}s)");
    Console.WriteLine();

    AppOrchestrator orchestrator = AppOrchestrator.CreateFromOptions(options);
    SimulationResult result = orchestrator.Run();

    PrintSummary(result, options.ControllerName);

    if (!string.IsNullOrWhiteSpace(options.OutputCsvPath))
    {
        AppendResultToCsv(options.OutputCsvPath, options, result);
        Console.WriteLine($"Results saved to: {options.OutputCsvPath}");
    }

    return 0;
}
catch (ArgumentException ex)
{
    Console.Error.WriteLine($"Invalid argument: {ex.Message}");
    Console.Error.WriteLine("Run with --help to see available options.");
    return 1;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Simulation error: {ex.Message}");
    return 1;
}

static CliOptions ParseArguments(string[] args)
{
    string? mapPath = null;
    string? demandPath = null;
    string controllerName = "noop";
    int seed = 42;
    double tickDurationSeconds = 1.0;
    double durationSeconds = 100.0;
    string? outputCsvPath = null;

    for (int i = 0; i < args.Length; i++)
    {
        string flag = args[i];

        string RequireValue()
        {
            if (i + 1 >= args.Length)
            {
                throw new ArgumentException($"Missing value for option '{flag}'.");
            }
            return args[++i];
        }

        switch (flag)
        {
            case "--map":
            case "-m":
                mapPath = RequireValue();
                break;

            case "--demand":
            case "-d":
                demandPath = RequireValue();
                break;

            case "--controller":
            case "-c":
                controllerName = RequireValue();
                if (!AppOrchestrator.SupportedControllers.Contains(controllerName))
                {
                    throw new ArgumentException(
                        $"Unknown controller '{controllerName}'. Supported controllers: {string.Join(", ", AppOrchestrator.SupportedControllers)}.");
                }
                break;

            case "--seed":
            case "-s":
                string seedRaw = RequireValue();
                if (!int.TryParse(seedRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out seed))
                {
                    throw new ArgumentException($"Invalid integer for {flag}: '{seedRaw}'.");
                }
                break;

            case "--duration":
            case "-t":
                string durationRaw = RequireValue();
                if (!double.TryParse(durationRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out durationSeconds) || durationSeconds <= 0)
                {
                    throw new ArgumentException($"Duration for {flag} must be a positive number: '{durationRaw}'.");
                }
                break;

            case "--tick":
                string tickRaw = RequireValue();
                if (!double.TryParse(tickRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out tickDurationSeconds) || tickDurationSeconds <= 0)
                {
                    throw new ArgumentException($"Tick duration for {flag} must be a positive number: '{tickRaw}'.");
                }
                break;

            case "--out":
            case "-o":
                outputCsvPath = RequireValue();
                break;

            default:
                throw new ArgumentException($"Unknown option: '{flag}'.");
        }
    }

    return new CliOptions(
        MapPath: mapPath,
        DemandPath: demandPath,
        ControllerName: controllerName,
        Seed: seed,
        TickDurationSeconds: tickDurationSeconds,
        DurationSeconds: durationSeconds,
        OutputCsvPath: outputCsvPath
    );
}

static void PrintSummary(SimulationResult result, string controllerName)
{
    SimulationMetrics m = result.Metrics;
    var inv = CultureInfo.InvariantCulture;

    Console.WriteLine("TrafficVillage — simulation complete");
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine($"  Controller:            {controllerName}");
    Console.WriteLine($"  Duration:              {result.DurationSeconds.ToString("F1", inv)} s");
    Console.WriteLine($"  Seed:                  {result.Seed}");
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine($"  Total vehicles exited: {m.TotalVehiclesExited}");
    Console.WriteLine($"  Throughput:            {m.ThroughputPerHour.ToString("F2", inv)} veh/h");
    Console.WriteLine($"  Average wait time:     {m.AverageWaitTimeSeconds.ToString("F2", inv)} s");
    Console.WriteLine($"  P95 wait time:         {m.P95WaitTimeSeconds.ToString("F2", inv)} s");
    Console.WriteLine($"  Max queue length:      {m.MaxQueueLength} veh");
    Console.WriteLine($"  Average stop count:    {m.AverageStopCount.ToString("F2", inv)} stops/veh");
    Console.WriteLine("--------------------------------------------------");
}

static void AppendResultToCsv(string path, CliOptions options, SimulationResult result)
{
    var inv = CultureInfo.InvariantCulture;
    SimulationMetrics m = result.Metrics;

    string? directory = Path.GetDirectoryName(path);
    if (!string.IsNullOrEmpty(directory))
        Directory.CreateDirectory(directory);

    bool fileExists = File.Exists(path);
    using var writer = new StreamWriter(path, append: true);

    if (!fileExists)
    {
        writer.WriteLine(
            "Controller,Seed,DurationSeconds,TickDurationSeconds," +
            "TotalVehiclesExited,ThroughputPerHour,AverageWaitTimeSeconds," +
            "P95WaitTimeSeconds,MaxQueueLength,AverageStopCount"
        );
    }

    writer.WriteLine(string.Join(",",
        options.ControllerName,
        result.Seed.ToString(inv),
        result.DurationSeconds.ToString("F2", inv),
        options.TickDurationSeconds.ToString("F2", inv),
        m.TotalVehiclesExited.ToString(inv),
        m.ThroughputPerHour.ToString("F2", inv),
        m.AverageWaitTimeSeconds.ToString("F2", inv),
        m.P95WaitTimeSeconds.ToString("F2", inv),
        m.MaxQueueLength.ToString(inv),
        m.AverageStopCount.ToString("F2", inv))
    );
}

static void PrintHelp()
{
    Console.WriteLine("""
        TrafficVillage — Traffic Simulation CLI

        Usage:
          dotnet run --project src/TrafficVillage -- [options]

        Options:
          -m, --map <path>          Path to road network map file
          -d, --demand <path>       Path to traffic demand profile file
          -c, --controller <name>   Traffic controller algorithm (default: noop)
          -s, --seed <int>          Random seed for reproducibility (default: 42)
          -t, --duration <seconds>  Simulation duration in seconds (default: 100)
              --tick <seconds>      Single tick duration in seconds (default: 1.0)
          -o, --out <path>          Append summary metrics to a CSV file
          -h, --help                Show this help message
        """);
}
