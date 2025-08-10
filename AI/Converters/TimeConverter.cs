using IO.Astrodynamics.TimeSystem;
using IO.MCP.AI.Models;
using Time = IO.MCP.AI.Models.Time;

namespace IO.MCP.AI.Converters;

public static class TimeConverter
{
    public static Time Convert(this Astrodynamics.TimeSystem.Time time)
    {
        DateKind kind = DateKind.TDB;
        switch (time.Frame.Name)
        {
            case "UTC":
                kind = DateKind.UTC;
                break;
            case "TDB":
                kind = DateKind.TDB;
                break;
            case "TDT":
                kind = DateKind.TDT;
                break;
            case "TAI":
                kind = DateKind.TAI;
                break;
            case "GPS":
                kind = DateKind.GPS;
                break;
            case "":
                kind = DateKind.Local;
                break;
            default:
                break;
        }

        return new Time(time.DateTime, kind);
    }

    public static Astrodynamics.TimeSystem.Time Convert(this Time time)
    {
        TimeFrame frame = null;
        switch (time.Kind)
        {
            case DateKind.UTC:
                frame = TimeFrame.UTCFrame;
                break;
            case DateKind.TDB:
                frame = TimeFrame.TDBFrame;
                break;
            case DateKind.TDT:
                frame = TimeFrame.TDTFrame;
                break;
            case DateKind.TAI:
                frame = TimeFrame.TAIFrame;
                break;
            case DateKind.GPS:
                frame = TimeFrame.GPSFrame;
                break;
            case DateKind.Local:
                frame = TimeFrame.LocalFrame;
                break;
            default:
                break;
        }

        return new Astrodynamics.TimeSystem.Time(time.DateTime, frame);
    }
}