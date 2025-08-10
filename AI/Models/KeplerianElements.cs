using IO.MCP.Data.SolarSystemObjects;

namespace IO.MCP.AI.Models;

public class KeplerianElements
{
    public KeplerianElements()
    {
        
    }

    public KeplerianElements(double semiMajorAxis, double eccentricity, double inclination, double argumentOfPeriapsis, double rightAscensionOfAscendingNode, double meanAnomaly, CelestialItemsEnum centerOfMotionId, Time epoch, FramesEnum frame)
    {
        CenterOfMotion = centerOfMotionId;
        Epoch = epoch;
        Frame = frame;
        SemiMajorAxis = semiMajorAxis;
        Eccentricity = eccentricity;
        Inclination = inclination;
        ArgumentOfPeriapsis = argumentOfPeriapsis;
        RightAscensionOfAscendingNode = rightAscensionOfAscendingNode;
        MeanAnomaly = meanAnomaly;
    }
  
    public CelestialItemsEnum CenterOfMotion { get; set; }
    public Time Epoch { get; set; }
    public FramesEnum Frame { get; set; }
    
    public double SemiMajorAxis { get; set; }
    public double Eccentricity { get; set; }
    public double Inclination { get; set; }
    public double ArgumentOfPeriapsis { get; set; }
    public double RightAscensionOfAscendingNode { get; set; }
    public double MeanAnomaly { get; set; }
}