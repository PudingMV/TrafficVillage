namespace TrafficVillage.Model;

public interface IDemandLoader
{
    DemandProfile Load(string path);
}
