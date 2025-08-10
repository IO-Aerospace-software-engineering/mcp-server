using System.ComponentModel;
using IO.Astrodynamics;
using IO.MCP.AI.Converters;
using IO.MCP.AI.Models;
using IO.MCP.Data.SolarSystemObjects;
using ModelContextProtocol.Server;
using Aberration = IO.Astrodynamics.Aberration;
using Frame = IO.Astrodynamics.Frames.Frame;
using Time = IO.Astrodynamics.TimeSystem.Time;
using Window = IO.Astrodynamics.TimeSystem.Window;

namespace IO.MCP.AI.Tools;

[McpServerToolType]
public class CelestialBodyTools
{
    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Get ephemeris as state vectors", UseStructuredContent = true)]
    [Description("Gets the state vectors of a celestial body observed from another celestial body in given frame and time range")]
    public static IEnumerable<Models.StateVector> GetEphemerisAsStateVectors(
        [Description("Celestial item observer name")]
        CelestialItemsEnum observerName,
        [Description("Celestial item target name")]
        CelestialItemsEnum targetName,
        [Description("Frame")] FramesEnum frame,
        [Description($"Start {Rules.TIME_RULE}")]
        string startTime,
        [Description($"End {Rules.TIME_RULE}")]
        string endTime,
        [Description(Rules.TIME_STEP_RULE)] double timeStep,
        [Description("Aberration correction.")]
        Aberration aberrationCorrection)
    {
        timeStep = System.Math.Max(timeStep, 1);
        var observerDomain = Astrodynamics.Body.CelestialItem.Create(MCP.Data.DataProvider.FindCelestialBody(observerName.ToString()).NaifId);
        var target = Astrodynamics.Body.CelestialItem.Create(MCP.Data.DataProvider.FindCelestialBody(targetName.ToString()).NaifId);
        var window = new Window(new Time(startTime), new Time(endTime));
        var frameDomain = frame == FramesEnum.ICRF ? Frame.ICRF : new Frame(frame.GetDescription());
        var ephemeris = API.Instance.ReadEphemeris(window, observerDomain, target, frameDomain,
            aberrationCorrection, TimeSpan.FromSeconds(timeStep));
        return ephemeris.Select(e =>
            new StateVector(observerName, e.Epoch.Convert(), frame,
                e.ToStateVector().Position.Convert(), e.ToStateVector().Velocity.Convert()));
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Get celestial body properties", UseStructuredContent = true)]
    [Description("Gets geophysical properties of a celestial body")]
    public static Models.CelestialBody GetCelestialBodyProperties(
        [Description("Celestial body name")] PlanetsMoonsEnum celestialBodyName)
    {
        var res = API.Instance.GetCelestialBodyInfo((int)celestialBodyName);
        return new Models.CelestialBody()
        {
            NaifId = res.Id,
            CenterOfMotionId = res.CenterOfMotionId,
            BarycenterOfMotionId = res.BarycenterOfMotionId,
            Name = res.Name,
            Radii = new Vector(res.Radii.X, res.Radii.Y, res.Radii.Z),
            GM = res.GM,
            FrameName = res.FrameName,
            FrameId = res.FrameId,
            J2 = res.J2,
            J3 = res.J3,
            J4 = res.J4
        };
    }
}