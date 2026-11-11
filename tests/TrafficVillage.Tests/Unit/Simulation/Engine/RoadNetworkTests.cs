using FluentAssertions;
using TrafficVillage.Simulation.Engine.RoadNetwork;

namespace TrafficVillage.Tests.Simulation.Engine;

public class RoadNetworkTests
{
    [Fact]
    public void Stub_network_has_no_nodes_or_edges()
    {
        var network = new RoadNetwork();

        network.Nodes.Should().BeEmpty();
        network.Edges.Should().BeEmpty();
        network.ControlledIntersectionIds.Should().BeEmpty();
    }
}
