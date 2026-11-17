namespace TrafficVillage.Model;

public interface IMapLoader
{
    IRoadNetwork Load(string path);
}
