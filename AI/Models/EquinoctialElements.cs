using IO.MCP.Data.SolarSystemObjects;

namespace IO.MCP.AI.Models;

public class EquinoctialElements
{
    public EquinoctialElements()
    {
        
    }
    
    public EquinoctialElements(double p, double f, double g, double h, double k, double l0, CelestialItemsEnum centerOfMotionId, Time epoch, FramesEnum frame)
    {
        CenterOfMotion = centerOfMotionId;
        Epoch = epoch;
        Frame = frame;
        P = p;
        F = f;
        G = g;
        H = h;
        K = k;
        L0 = l0;
    }
    
    public CelestialItemsEnum CenterOfMotion { get; set; }
    public Time Epoch { get; set; }
    public FramesEnum Frame { get; set; }
    
    /// <summary>
    /// P
    /// </summary>
    public double P { get; set; }

    /// <summary>
    /// F
    /// </summary>
    public double F { get; set; }

    /// <summary>
    /// G
    /// </summary>
    public double G { get; set; }

    /// <summary>
    /// H
    /// </summary>
    public double H { get; set; }

    /// <summary>
    /// K
    /// </summary>
    public double K { get; set; }

    /// <summary>
    /// L0
    /// </summary>
    public double L0 { get; set; }
}