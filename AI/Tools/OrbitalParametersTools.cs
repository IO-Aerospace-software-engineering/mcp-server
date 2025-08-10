using System.ComponentModel;
using IO.Astrodynamics;
using IO.MCP.AI.Converters;
using IO.MCP.AI.Models;
using IO.MCP.Data.SolarSystemObjects;
using ModelContextProtocol.Server;
using Equatorial = IO.MCP.AI.Models.Equatorial;
using EquinoctialElements = IO.MCP.AI.Models.EquinoctialElements;
using Frame = IO.Astrodynamics.Frames.Frame;
using KeplerianElements = IO.MCP.AI.Models.KeplerianElements;
using StateVector = IO.MCP.AI.Models.StateVector;

namespace IO.MCP.AI.Tools;

[McpServerToolType]
public class OrbitalParametersTools
{
    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert state vector to Keplerian elements", UseStructuredContent = true)]
    [Description("Converts a state vector to Keplerian elements")]
    public static KeplerianElements ConvertStateVectorToKeplerianElements([Description("Statevector to convert")] StateVector stateVector)
    {
        Astrodynamics.OrbitalParameters.StateVector stateVectorDomain = new Astrodynamics.OrbitalParameters.StateVector(stateVector.Position.Convert(), stateVector.Velocity.Convert(),
            Astrodynamics.Body.CelestialItem.Create((int)stateVector.CenterOfMotion), stateVector.Epoch.Convert(), new Frame(stateVector.Frame.GetDescription()));
        var res = stateVectorDomain.ToKeplerianElements();
        return new KeplerianElements(res.A, res.E, res.I, res.AOP, res.RAAN, res.M, stateVector.CenterOfMotion, stateVector.Epoch, stateVector.Frame);
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert state vector to Equinoctial elements", UseStructuredContent = true)]
    [Description("Converts a state vector to Equinoctial elements")]
    public static EquinoctialElements ConvertStateVectorToEquinoctialElements([Description("Statevector to convert")] StateVector stateVector)
    {
        Astrodynamics.OrbitalParameters.StateVector stateVectorDomain = new Astrodynamics.OrbitalParameters.StateVector(stateVector.Position.Convert(), stateVector.Velocity.Convert(),
            Astrodynamics.Body.CelestialItem.Create((int)stateVector.CenterOfMotion), stateVector.Epoch.Convert(), new Frame(stateVector.Frame.GetDescription()));
        var res = stateVectorDomain.ToEquinoctial();
        return new EquinoctialElements(res.P, res.F, res.G, res.H, res.K, res.L0, stateVector.CenterOfMotion, stateVector.Epoch, stateVector.Frame);
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert state vector to Equatorial coordinates", UseStructuredContent = true)]
    [Description("Converts a state vector to Equatorial coordinates")]
    public static Equatorial ConvertStateVectorToEquatorialCoordinates([Description("Statevector to convert")] StateVector stateVector)
    {
        Astrodynamics.OrbitalParameters.StateVector stateVectorDomain = new Astrodynamics.OrbitalParameters.StateVector(stateVector.Position.Convert(), stateVector.Velocity.Convert(),
            Astrodynamics.Body.CelestialItem.Create((int)stateVector.CenterOfMotion), stateVector.Epoch.Convert(), new Frame(stateVector.Frame.GetDescription()));
        var res = stateVectorDomain.ToFrame(Frame.ICRF).ToEquatorial();
        return new Equatorial(res.RightAscension, res.Declination, res.Distance, res.Epoch.Convert());
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert Keplerian elements to state vector", UseStructuredContent = true)]
    [Description("Converts keplerian elements to state vector")]
    public static StateVector ConvertKeplerianElementsToStateVector([Description("Keplerian elements to convert")] KeplerianElements keplerianElements)
    {
        Astrodynamics.OrbitalParameters.KeplerianElements keplerianElementsDomain = new Astrodynamics.OrbitalParameters.KeplerianElements(keplerianElements.SemiMajorAxis, keplerianElements.Eccentricity,
            keplerianElements.Inclination, keplerianElements.ArgumentOfPeriapsis, keplerianElements.RightAscensionOfAscendingNode, keplerianElements.MeanAnomaly,
            Astrodynamics.Body.CelestialItem.Create((int)keplerianElements.CenterOfMotion), keplerianElements.Epoch.Convert(), new Frame(keplerianElements.Frame.GetDescription()));
        var res = keplerianElementsDomain.ToStateVector();
        return new StateVector(keplerianElements.CenterOfMotion, keplerianElements.Epoch, keplerianElements.Frame, new Vector(res.Position.X, res.Position.Y, res.Position.Z),
            new Vector(res.Velocity.X, res.Velocity.Y, res.Velocity.Z));
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert equinoctial elements to state vector", UseStructuredContent = true)]
    [Description("Converts equinoctial elements to state vector")]
    public static StateVector ConvertEquinoctialElementsToStateVector([Description("Equinoctial elements to convert")] EquinoctialElements equinoctialElements)
    {
        Astrodynamics.OrbitalParameters.EquinoctialElements equinoctialElementsDomain = new Astrodynamics.OrbitalParameters.EquinoctialElements(equinoctialElements.P, equinoctialElements.F,
            equinoctialElements.G,
            equinoctialElements.H, equinoctialElements.K, equinoctialElements.L0, Astrodynamics.Body.CelestialItem.Create((int)equinoctialElements.CenterOfMotion),
            equinoctialElements.Epoch.Convert(),
            new Frame(equinoctialElements.Frame.GetDescription()));
        var res = equinoctialElementsDomain.ToStateVector();
        return new StateVector(equinoctialElements.CenterOfMotion, equinoctialElements.Epoch, equinoctialElements.Frame, new Vector(res.Position.X, res.Position.Y, res.Position.Z),
            new Vector(res.Velocity.X, res.Velocity.Y, res.Velocity.Z));
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert state vector to the given frame", UseStructuredContent = true)]
    [Description("Converts state vector to the given frame")]
    public static StateVector ConvertStateVectorToTheGivenFrame([Description("State vector to convert")] StateVector stateVector, [Description("Target frame")] FramesEnum frame)
    {
        Astrodynamics.OrbitalParameters.StateVector stateVectorDomain = new Astrodynamics.OrbitalParameters.StateVector(stateVector.Position.Convert(), stateVector.Velocity.Convert(),
            Astrodynamics.Body.CelestialItem.Create((int)stateVector.CenterOfMotion), stateVector.Epoch.Convert(), new Frame(stateVector.Frame.GetDescription()));
        var targetFrame = new Frame(frame.GetDescription());
        var res = stateVectorDomain.ToFrame(targetFrame).ToStateVector();
        return new StateVector(stateVector.CenterOfMotion, stateVector.Epoch, stateVector.Frame, new Vector(res.Position.X, res.Position.Y, res.Position.Z),
            new Vector(res.Velocity.X, res.Velocity.Y, res.Velocity.Z));
    }
}