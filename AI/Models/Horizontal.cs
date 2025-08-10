namespace IO.MCP.AI.Models;

/// <summary>
/// Horizontal coordinates
/// </summary>
public class Horizontal
{
    public Horizontal()
    {
    }

    public Horizontal(double azimuth, double elevation, double range, Time epoch)
    {
        Elevation = elevation;
        Azimuth = azimuth;
        Range = range;
        Epoch = epoch;
    }

    /// <summary>
    /// Elevation
    /// </summary>
    public double Elevation { get; set; }

    /// <summary>
    /// Azimuth
    /// </summary>
    public double Azimuth { get; set; }

    /// <summary>
    /// Range
    /// </summary>
    public double Range { get; set; }

    /// <summary>
    /// Epoch
    /// </summary>
    public Time Epoch { get; set; }
}