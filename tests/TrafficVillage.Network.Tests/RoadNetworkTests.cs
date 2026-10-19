using FluentAssertions;
using TrafficVillage.Network;

namespace TrafficVillage.Network.Tests;

public class RoadNetworkTests
{
    [Fact]
    public void Stub_network_has_no_nodes()
    {
        var network = new RoadNetwork();

        network.NodeIds.Should().BeEmpty();
    }
}
