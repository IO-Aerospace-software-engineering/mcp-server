using System.ComponentModel;
using System.Data;
using IO.Astrodynamics;
using IO.MCP.AI.Converters;
using IO.MCP.AI.Models;
using IO.MCP.Data.SolarSystemObjects;
using ModelContextProtocol.Server;
using Aberration = IO.Astrodynamics.Aberration;
using Planetodetic = IO.MCP.AI.Models.Planetodetic;
using StateVector = IO.MCP.AI.Models.StateVector;

namespace IO.MCP.AI.Tools;

[McpServerToolType]
public class SiteTools
{
    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Get planetodetic coordinates of a deep space station (DSS)", UseStructuredContent = true)]
    [Description(
        "Get the planetodetic coordinates of a deep space station (DSS). The planetodetic coordinates are the latitude in radian, longitude in radian and altitude in meters of the station.")]
    public static Planetodetic GetDeepSpaceStationPlanetodeticCoordinates([Description("The deep space station")] GroundStationsEnum dss)
    {
        int dssId = (int)dss - 399000;
        var celestialBody = new IO.Astrodynamics.Body.CelestialBody(399);
        var site = new IO.Astrodynamics.Surface.Site(dssId, $"DSS-{dssId}", celestialBody);
        return new Planetodetic(site.Planetodetic.Latitude, site.Planetodetic.Longitude, site.Planetodetic.Altitude);
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Get state vector of a deep space station (DSS)", UseStructuredContent = true)]
    [Description("Get the state vector of a deep space station (DSS) observed from a celestial body in a given frame and time.")]
    public static StateVector GetDeepSpaceStationStateVector(
        [Description("The deep space station")]
        GroundStationsEnum dss,
        [Description("The celestial body from which the state vector is observed")]
        CelestialItemsEnum observer,
        [Description("Frame")] FramesEnum frame,
        [Description($"{Rules.TIME_RULE}")] string datetime,
        [Description("Aberration correction.")]
        Aberration aberrationCorrection)
    {
        int dssId = (int)dss - 399000;
        var celestialBody = new IO.Astrodynamics.Body.CelestialBody(399);
        var site = new IO.Astrodynamics.Surface.Site(dssId, $"DSS-{dssId}", celestialBody);
        var observerDomain = Astrodynamics.Body.CelestialItem.Create((int)observer);
        var frameInstance = new IO.Astrodynamics.Frames.Frame(frame.GetDescription());
        var time = new IO.Astrodynamics.TimeSystem.Time(datetime);
        var res = API.Instance.ReadEphemeris(time, observerDomain, site, frameInstance, aberrationCorrection);
        return new StateVector(observer, res.Epoch.Convert(), frame, res.ToStateVector().Position.Convert(), res.ToStateVector().Velocity.Convert());
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Get horizontal coordinates of a celestial body from a deep space station (DSS)", UseStructuredContent = true)]
    [Description("Get the horizontal coordinates of a celestial body from a deep space station (DSS) at given epoch.")]
    public static Horizontal GetHorizontalCoordinates([Description("The deep space station")] GroundStationsEnum dss,
        [Description("Celestial body target")] CelestialItemsEnum celestialBodyTarget,
        [Description($"{Rules.TIME_RULE}")] string datetime,
        [Description("Aberration correction.")]
        Aberration aberrationCorrection)
    {
        int dssId = (int)dss - 399000;
        var earth = new IO.Astrodynamics.Body.CelestialBody(399);
        var site = new IO.Astrodynamics.Surface.Site(dssId, $"DSS-{dssId}", earth);
        var target = Astrodynamics.Body.CelestialItem.Create((int)celestialBodyTarget);
        var time = new IO.Astrodynamics.TimeSystem.Time(datetime);
        var res = site.GetHorizontalCoordinates(time, target, aberrationCorrection);
        return new Horizontal(res.Azimuth, res.Elevation, res.Range, res.Epoch.Convert());
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Get the frame of a deep space station (DSS)", UseStructuredContent = true)]
    [Description("Find Deep space station frame")]
    public static string GetDSSFrame([Description("The deep space station")] GroundStationsEnum dss)
    {
        var res = MCP.Data.DataProvider.FindGroundStation(dss.ToString());
        if (string.IsNullOrEmpty(res.Name))
        {
            throw new DataException("Deep space station not found");
        }

        return res.Frame;
    }
}