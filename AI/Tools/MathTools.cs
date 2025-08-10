using System.ComponentModel;
using IO.Astrodynamics;
using ModelContextProtocol.Server;

namespace IO.MCP.AI.Tools;

[McpServerToolType()]
public class MathTools
{
    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert degrees to radains")]
    [Description("Convert degrees to radians")]
    public static double DegreesToRadians([Description("Degrees to convert")] double degrees)
    {
        return degrees * Constants.Deg2Rad;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert radians to degrees")]
    [Description("Convert radians to degrees")]
    public static double RadiansToDegrees([Description("Radians to convert")] double radians)
    {
        return radians * Constants.Rad2Deg;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert degrees to hours")]
    [Description("Convert degrees to hours")]
    public static double ConvertDegreesToHours([Description("Degrees to convert")] double degrees)
    {
        return degrees / 15.0;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert hours to degrees")]
    [Description("Convert hours to degrees")]
    public static double ConvertHoursToDegrees([Description("Hours to convert")] double hours)
    {
        return hours * 15.0;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert degrees to arcseconds")]
    [Description("Convert degrees to arcseconds")]
    public static double DegreesToArcseconds([Description("Degrees to convert")] double degrees)
    {
        return degrees * 3600.0;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert arcseconds to degrees")]
    [Description("Convert arcseconds to degrees")]
    public static double ArcsecondsToDegrees([Description("Arcseconds to convert")] double arcseconds)
    {
        return arcseconds / 3600.0;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert radians to arcseconds")]
    [Description("Convert radians to arcseconds")]
    public static double RadiansToArcseconds([Description("Radians to convert")] double radians)
    {
        return radians * 206_264.80624709636;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert arcseconds to radians")]
    [Description("Convert arcseconds to radians")]
    public static double ArcsecondsToRadians([Description("Arcseconds to convert")] double arcseconds)
    {
        return arcseconds / 206_264.80624709636;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert meters to miles")]
    [Description("Convert meters to miles")]
    public static double MetersToMiles([Description("Meters to convert")] double meters)
    {
        return meters / 1609.344;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert miles to meters")]
    [Description("Convert miles to meters")]
    public static double MilesToMeters([Description("Miles to convert")] double miles)
    {
        return miles * 1609.344;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert meters to feet")]
    [Description("Convert meters to feet")]
    public static double MetersToFeet([Description("Meters to convert")] double meters)
    {
        return meters * 3.28084;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert feet to meters")]
    [Description("Convert feet to meters")]
    public static double FeetToMeters([Description("Feet to convert")] double feet)
    {
        return feet / 3.28084;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert meters to kilometers")]
    [Description("Convert meters to kilometers")]
    public static double MetersToKilometers([Description("Meters to convert")] double meters)
    {
        return meters / 1000.0;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert kilometers to meters")]
    [Description("Convert kilometers to meters")]
    public static double KilometersToMeters([Description("Kilometers to convert")] double kilometers)
    {
        return kilometers * 1000.0;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert meters to astronomical units")]
    [Description("Convert meters to astronomical units")]
    public static double MetersToAstronomicalUnits([Description("Meters to convert")] double meters)
    {
        return meters / 149_597_870_700.0;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert astronomical units to meters")]
    [Description("Convert astronomical units to meters")]
    public static double AstronomicalUnitsToMeters([Description("Astronomical units to convert")] double astronomicalUnits)
    {
        return astronomicalUnits * 149_597_870_700.0;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert meters to parsec")]
    [Description("Convert meters to parsec")]
    public static double MetersToParsec([Description("Meters to convert")] double meters)
    {
        return meters / Constants.Parsec2Meters;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert parsec to meters")]
    [Description("Convert parsec to meters")]
    public static double ParsecToMeters([Description("Parsec to convert")] double parsec)
    {
        return parsec * Constants.Parsec2Meters;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert meters to light years")]
    [Description("Convert meters to light years")]
    public static double MetersToLightYears([Description("Meters to convert")] double meters)
    {
        return meters / 299792458.0;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert light years to meters")]
    [Description("Convert light years to meters")]
    public static double LightYearsToMeters([Description("Light years to convert")] double lightYears)
    {
        return lightYears * 299792458.0;
    }
}