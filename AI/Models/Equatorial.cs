
namespace IO.MCP.AI.Models;

public class Equatorial
{
    public Equatorial()
    {
        
    }
    
    public Equatorial(double rightAscension, double declination, double range, Time epoch)
    {
        RightAscension = rightAscension;
        Declination = declination;
        Range = range;
        Epoch = epoch;
    }
    public double RightAscension { get; set; }
    public double Declination { get; set; }
    public double Range { get; set; }
    public Time Epoch { get; set; }
}