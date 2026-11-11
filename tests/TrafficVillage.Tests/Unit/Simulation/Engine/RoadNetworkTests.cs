using FluentAssertions;
using TrafficVillage.Simulation.Engine.RoadNetwork;

namespace TrafficVillage.Tests.Simulation.Engine;

public class RoadNetworkTests
{
    [Fact]
    public void Stub_factories_throw_not_implemented()
    {
        // shortcut: factory stubs throw NotImplementedException, replace with node/edge assertions once implemented
        var generate = () => RoadNetwork.GenerateRandom(4);
        var fromMap = () => RoadNetwork.FromMap();

        generate.Should().Throw<NotImplementedException>();
        fromMap.Should().Throw<NotImplementedException>();
    }
}
