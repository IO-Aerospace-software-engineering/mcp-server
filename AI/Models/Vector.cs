using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace IO.MCP.AI.Models;

[DataContract]
public class Vector
{
    public Vector()
    {
        
    }
    public Vector(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    public Vector(string strVector)
    {
        double[] data = strVector.Split(" ").Select(double.Parse).ToArray();
        if (data.Length != 3)
        {
            throw new ArgumentException("Invalid string vector");
        }
        X = data[0];
        Y = data[1];
        Z = data[2];
    }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double X { get; set; }
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double Y { get; set; }
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double Z { get; set; }

    public double Magnitude()
    {
        return System.Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public override string ToString()
    {
        return $"{nameof(X)}: {X:F3}, {nameof(Y)}: {Y:F3}, {nameof(Z)}: {Z:F3}";
    }
    public string ToDisplay()
    {
        return $"{X} {Y} {Z}";
    }
}