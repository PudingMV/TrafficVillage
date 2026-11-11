namespace TrafficVillage.Model;

/// <summary>
/// Directed road segment. A two-way street is two edges in opposite directions.
/// </summary>
/// 
/// <param name="From">The starting node of the edge.</param>
/// <param name="To">The ending node of the edge.</param>
/// <param name="Length">The length of the edge in ticks. Means how many simulation ticks vehicle must spend (moving) 
/// on this edge in order to pass it.</param>
public sealed record Edge(
    NodeId From,
    NodeId To,
    int Length);
