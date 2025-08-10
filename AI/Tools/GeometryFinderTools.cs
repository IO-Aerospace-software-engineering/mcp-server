using System.ComponentModel;
using IO.Astrodynamics;
using IO.MCP.AI.Converters;
using IO.MCP.AI.Models;
using IO.MCP.Data.SolarSystemObjects;
using ModelContextProtocol.Server;
using Aberration = IO.Astrodynamics.Aberration;
using Frame = IO.Astrodynamics.Frames.Frame;

namespace IO.MCP.AI.Tools;

[McpServerToolType]
public class GeometryFinderTools
{
    private const double RATIO = 3000.0;

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Find datetime windows when a coordinate constraint is met", UseStructuredContent = true)]
    [Description("Find datetime windows when a coordinate constraint is met")]
    public static IEnumerable<Window> FindCoordinateConstraint(
        [Description("Search window")] Window searchWindow,
        [Description("Celestial item observer name")]
        CelestialItemsEnum observerName,
        [Description("Celestial item target name")]
        CelestialItemsEnum targetName,
        [Description("Reference frame")] FramesEnum frame,
        [Description("Coordinate system")] CoordinateSystem coordinateSystem,
        [Description("Coordinate")] Coordinate coordinate,
        [Description("Relational operator")] RelationnalOperator relationalOperator,
        [Description("Value to evaluate")] double value,
        [Description("Tolerance value")] double adjustValue,
        [Description("Aberration")] Aberration aberrationCorrection)
    {
        return API.Instance.FindWindowsOnCoordinateConstraint(searchWindow.Convert(), (int)observerName, (int)targetName, new Frame(frame.GetDescription()), coordinateSystem,
            coordinate,
            relationalOperator, value, adjustValue, aberrationCorrection, searchWindow.Length / RATIO).Select(x => x.Convert());
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Find datetime windows when a distance constraint is met", UseStructuredContent = true)]
    [Description("Find datetime windows when a distance constraint is met")]
    public static IEnumerable<Window> FindDistanceConstraint(
        [Description("Search window")] Window searchWindow,
        [Description("Celestial item observer name")]
        CelestialItemsEnum observerName,
        [Description("Celestial item target name")]
        CelestialItemsEnum targetName,
        [Description("Relational operator")] RelationnalOperator relationalOperator,
        [Description("Value to evaluate")] double value,
        [Description("Aberration")] Aberration aberrationCorrection)
    {
        return API.Instance
            .FindWindowsOnDistanceConstraint(searchWindow.Convert(), (int)observerName, (int)targetName, relationalOperator, value, aberrationCorrection,
                searchWindow.Length / RATIO)
            .Select(x => x.Convert());
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Find datetime windows when a occulting constraint or eclipse is met", UseStructuredContent = true)]
    [Description("Find datetime windows when a occulting constraint or eclipse is met")]
    public static IEnumerable<Window> FindOccultingConstraint(
        [Description("Search window")] Window searchWindow,
        [Description("Celestial item observer name")]
        CelestialItemsEnum observerName,
        [Description("Occulted body is the body that is occulted by the occulting body")]
        CelestialItemsEnum occultedBody,
        [Description("Occulting body is the body that occults the occulted body")]
        PlanetsMoonsEnum occultingBody,
        [Description("Kind of occultating")] OccultationType occultingKind,
        [Description("Aberration")] Aberration aberrationCorrection)
    {
        var occultedBodyShape = DefineShape((int)occultedBody);
        var occultingBodyShape = DefineShape((int)occultingBody);
        return API.Instance
            .FindWindowsOnOccultationConstraint(searchWindow.Convert(), (int)observerName, (int)occultedBody, occultedBodyShape, (int)occultingBody, occultingBodyShape,
                occultingKind, aberrationCorrection, searchWindow.Length / RATIO)
            .Select(x => x.Convert());
    }

    private static ShapeType DefineShape(int occultedBodyNaifId)
    {
        var occultedBody = API.Instance.GetCelestialBodyInfo(occultedBodyNaifId);
        ShapeType occultedBodyShape = ShapeType.Point;
        if (occultedBody.Radii.X != 0 || occultedBody.Radii.Y != 0 || occultedBody.Radii.Z != 0)
        {
            occultedBodyShape = ShapeType.Ellipsoid;
        }

        return occultedBodyShape;
    }
}