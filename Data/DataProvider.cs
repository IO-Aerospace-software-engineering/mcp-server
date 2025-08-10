using IO.MCP.Data.SolarSystemObjects;

namespace IO.MCP.Data;

public class DataProvider
{
    public DataProvider()
    {
    }

    public static IEnumerable<NaifObject> PlanetsAndMoons()
    {
        return NaifObjectProvider<PlanetsAndMoons>.Get();
    }

    public static IEnumerable<NaifObject> Asteroids()
    {
        return NaifObjectProvider<Asteroids>.Get();
    }

    public static IEnumerable<NaifObject> Barycenters()
    {
        return NaifObjectProvider<Barycenters>.Get();
    }

    public static IEnumerable<NaifObject> Comets()
    {
        return NaifObjectProvider<Comets>.Get();
    }

    public static IEnumerable<NaifObject> GroundStations()
    {
        return NaifObjectProvider<GroundStations>.Get();
    }

    public static IEnumerable<NaifObject> LagrangePoints()
    {
        return NaifObjectProvider<LagrangePoints>.Get();
    }

    public static IEnumerable<NaifObject> Stars()
    {
        return NaifObjectProvider<Stars>.Get();
    }

    public static IEnumerable<NaifObject> InertialFrames()
    {
        return NaifObjectProvider<InertialFrames>.Get();
    }

    public static FileInfo GetSphericalHarmonicsFile()
    {
        return new FileInfo("./Data/SolarSystem/EGM2008_to70_TideFree");
    }

    public static IEnumerable<NaifObject> GetCelestialBodies()
    {
        var all = PlanetsAndMoons().Concat(Asteroids()).Concat(Barycenters()).Concat(Comets()).Concat(GroundStations())
            .Concat(LagrangePoints()).Concat(Stars());
        return all;
    }

    public static IEnumerable<NaifObject> GetAllNaifObjects()
    {
        var all = PlanetsAndMoons().Concat(Asteroids()).Concat(Barycenters()).Concat(Comets()).Concat(GroundStations())
            .Concat(LagrangePoints()).Concat(Stars()).Concat(InertialFrames());
        return all;
    }

    public static NaifObject FindCelestialBody(string name)
    {
        return Find((int)Enum.Parse<CelestialItemsEnum>(name, true));
    }

    public static NaifObject FindGroundStation(string name)
    {
        return Find((int)Enum.Parse<GroundStationsEnum>(name, true));
    }

    public static NaifObject Find(int naifIdentifier)
    {
        return GetAllNaifObjects().FirstOrDefault(x => x.NaifId == naifIdentifier);
    }

    public static IEnumerable<(string id, string name)> Frames()
    {
        var frames = InertialFrames().Select(x => (x.Frame, x.Name));
        frames = frames.Concat(Stars().Select(x => (x.Frame, x.Name)));
        frames = frames.Concat(PlanetsAndMoons().Select(x => (x.Frame, x.Name)));
        return frames;
    }
}