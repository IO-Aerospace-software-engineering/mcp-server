using System.ComponentModel;
using System.Text.Json.Serialization;

namespace IO.MCP.AI.Models;

public class CelestialBody
{
    [Description("Celestial body Naif Identifier")]
    public int NaifId { get; set; }

    [Description("Center of motion Naif Identifier")]
    public int CenterOfMotionId{ get; set; }

    [Description("Barycenter of motion Naif Identifier")]
    public int BarycenterOfMotionId{ get; set; }
    
    [Description("Celestial body name")]
    public string Name{ get; set; }

    [Description("Celestial body radius vector. X = equatorial radius,Z= polar radius,Y=equatorial radius")]
    public Vector Radii{ get; set; }

    [Description("Celestial body gravitational parameter")]
    public double GM{ get; set; }
    
    [Description("Celestial body fixed frame")]
    public string FrameName{ get; set; }
    
    [Description("Celestial body fixed frame Naif Identifier")]
    public int FrameId{ get; set; }
    
    [Description("Celestial body J2 coefficient")]
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double J2{ get; set; }
    
    [Description("Celestial body J3 coefficient")]
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double J3{ get; set; }
    
    [Description("Celestial body J4 coefficient")]
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double J4{ get; set; }
}