using FluentAssertions;
using TrafficVillage.Metrics;
using TrafficVillage.Model;

namespace TrafficVillage.Tests.Metrics;

public class StandardMetricsCollectorTests
{
    private sealed class FakeVehicle(VehicleId id, int waitTime = 0) : IVehicle
    {
        public VehicleId Id { get; } = id;
        public NodeId Origin => default!;
        public NodeId Destination => default!;
        public IReadOnlyList<Edge> Route => [];
        public Edge CurrentEdge => default!;
        public int RemainingEdgeTicks => 0;
        public int RouteTime => 0;
        public int WaitTime { get; set; } = waitTime;
    }

    [Fact]
    public void Finalize_on_empty_run_returns_zero_metrics()
    {
        var collector = new StandardMetricsCollector(tickDurationSeconds: 1.0);
        var state = new NetworkState(
            CurrentTimeSeconds: 10.0,
            Vehicles: [],
            Intersections: new Dictionary<NodeId, IntersectionState>()
            );

        collector.OnTick(state);
        var metrics = collector.Finalize();

        metrics.TotalVehiclesExited.Should().Be(0);
        metrics.AverageWaitTimeSeconds.Should().Be(0);
        metrics.P95WaitTimeSeconds.Should().Be(0);
        metrics.MaxQueueLength.Should().Be(0);
        metrics.ThroughputPerHour.Should().Be(0);
        metrics.AverageStopCount.Should().Be(0);
    }

    [Fact]
    public void Finalize_calculates_wait_times_and_throughput_for_two_exited_vehicles()
    {
        var collector = new StandardMetricsCollector(tickDurationSeconds: 2.0);
        var v1 = new FakeVehicle(new VehicleId("v1"), waitTime: 5);
        var v2 = new FakeVehicle(new VehicleId("v2"), waitTime: 15);

        collector.OnTick(new NetworkState(
            CurrentTimeSeconds: 3600.0,
            Vehicles: [],
            Intersections: new Dictionary<NodeId, IntersectionState>()
        ));

        collector.OnVehicleExited(v1);
        collector.OnVehicleExited(v2);

        var metrics = collector.Finalize();

        metrics.TotalVehiclesExited.Should().Be(2);
        metrics.AverageWaitTimeSeconds.Should().Be(20.0);
        metrics.P95WaitTimeSeconds.Should().Be(30.0);
        metrics.ThroughputPerHour.Should().Be(2.0);
    }

    [Fact]
    public void OnTick_tracks_stop_count_of_two_for_wait_then_move_then_wait()
    {
        var collector = new StandardMetricsCollector(tickDurationSeconds: 1.0);
        var vehicle = new FakeVehicle(new VehicleId("v1"), waitTime: 0);
        var emptyIntersections = new Dictionary<NodeId, IntersectionState>();

        vehicle.WaitTime = 1;
        collector.OnTick(new NetworkState(1.0, [vehicle], emptyIntersections));

        vehicle.WaitTime = 2;
        collector.OnTick(new NetworkState(2.0, [vehicle], emptyIntersections));

        collector.OnTick(new NetworkState(3.0, [vehicle], emptyIntersections));

        vehicle.WaitTime = 3;
        collector.OnTick(new NetworkState(4.0, [vehicle], emptyIntersections));

        collector.OnVehicleExited(vehicle);
        var metrics = collector.Finalize();

        metrics.AverageStopCount.Should().Be(2.0);
        metrics.AverageWaitTimeSeconds.Should().Be(3.0);
    }

    [Fact]
    public void OnTick_tracks_max_queue_length_rising_from_2_to_5()
    {
        var collector = new StandardMetricsCollector(tickDurationSeconds: 1.0);
        var nodeId = new NodeId("n1");
        var movementA = new MovementId("m1");
        var movementB = new MovementId("m2");

        var stateTick1 = new NetworkState(
            CurrentTimeSeconds: 1.0,
            Vehicles: [],
            Intersections: new Dictionary<NodeId, IntersectionState>
            {
                [nodeId] = new IntersectionState(
                    nodeId,
                    default!,
                    default,
                    TimeInPhaseSeconds: 1.0,
                    QueueLengths: new Dictionary<MovementId, int>
                    {
                        [movementA] = 2,
                        [movementB] = 1
                    }
                )
            }
        );

        var stateTick2 = new NetworkState(
            CurrentTimeSeconds: 2.0,
            Vehicles: [],
            Intersections: new Dictionary<NodeId, IntersectionState>
            {
                [nodeId] = new IntersectionState(
                    nodeId,
                    default!,
                    default,
                    TimeInPhaseSeconds: 2.0,
                    QueueLengths: new Dictionary<MovementId, int>
                    {
                        [movementA] = 3,
                        [movementB] = 5
                    }
                )
            }
        );

        collector.OnTick(stateTick1);
        collector.OnTick(stateTick2);

        var metrics = collector.Finalize();

        metrics.MaxQueueLength.Should().Be(5);
    }
}
