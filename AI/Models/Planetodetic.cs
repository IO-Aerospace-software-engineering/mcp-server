using System.ComponentModel.DataAnnotations;

namespace IO.MCP.AI.Models;

public class Planetodetic
{
    [Required] [Range(-90.0, 90.0)]public double? Latitude { get; set; }
    [Required] [Range(-180.0, 180.0)] public double? Longitude { get; set; }

    [Required] public double? Elevation { get; set; }
    public Planetodetic()
    {
    }

    public Planetodetic(double? latitude, double? longitude, double? elevation)
    {
        Latitude = latitude;
        Longitude = longitude;
        Elevation = elevation;
    }

    public override string ToString()
    {
        return $"{nameof(Latitude)}: {Latitude?.ToString("F3")}, {nameof(Longitude)}: {Longitude?.ToString("F3")}, {nameof(Elevation)}: {Elevation?.ToString("F3")}";
    }
}