using IO.Astrodynamics.Math;
using IO.MCP.AI.Models;

namespace IO.MCP.AI.Converters;

public static class VectorConverter
{
    public static Vector Convert(this in Vector3 vector3)
    {
        return new Vector(vector3.X, vector3.Y, vector3.Z);
    }
    
    public static Vector3 Convert(this Vector vector3)
    {
        return new Vector3(vector3.X, vector3.Y, vector3.Z);
    }
}