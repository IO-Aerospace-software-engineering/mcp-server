using System.ComponentModel;
using System.Runtime.Serialization;
using IO.MCP.Data.SolarSystemObjects;

namespace IO.MCP.AI.Models;

[DataContract]
public class StateVector
{
    [Description("The center of motion for the state vector, typically a celestial item.")]
    [DataMember(Name = "centerOfMotion", Order = 1)]
    public CelestialItemsEnum CenterOfMotion { get; set; }
    
    [Description($"{Rules.TIME_RULE}")]
    [DataMember(Name = "epoch", Order = 2)]
    public Time Epoch { get; set; }
    
    [Description("The frame in which the state vector is defined")]
    [DataMember(Name = "frame", Order = 3)]
    public FramesEnum Frame { get; set; }
    
    [Description("The position vector of the state vector in the specified frame.")]
    [DataMember(Name = "position", Order = 4)]
    public Vector Position { get; set; }
    
    [Description("The velocity vector of the state vector in the specified frame.")]
    [DataMember(Name = "velocity", Order = 5)]
    public Vector Velocity { get; set; }

    public StateVector()
    {
    }

    public StateVector(CelestialItemsEnum centerOfMotionId, Time epoch, FramesEnum frame, Vector position, Vector velocity)
    {
        CenterOfMotion = centerOfMotionId;
        Epoch = epoch;
        Frame = frame;
        Position = position;
        Velocity = velocity;
    }

    public override string ToString()
    {
        return $"{base.ToString()}; {nameof(Position)}: {Position}; {nameof(Velocity)}: {Velocity}";
    }
}