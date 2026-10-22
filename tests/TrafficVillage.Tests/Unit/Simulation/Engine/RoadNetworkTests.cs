using FluentAssertions;
using TrafficVillage.Simulation.Engine.RoadNetwork;

namespace TrafficVillage.Tests.Simulation.Engine;

public class RoadNetworkTests
{
    [Fact]
    public void Stub_network_has_no_nodes()
    {
        var network = new RoadNetwork();

        network.NodeIds.Should().BeEmpty();
    }
}
