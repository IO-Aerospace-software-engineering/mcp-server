using IO.MCP.AI.Converters;
using IO.MCP.AI.Models;

namespace IO.MCP.AI;

public static class TimeExtension
{
    public static Time ToTDB(this Time time)
    {
        return time.Convert().ToTDB().Convert();
    }
    
    public static Time ToUTC(this Time time)
    {
        return time.Convert().ToUTC().Convert();
    }
    
    public static Time ToTDT(this Time time)
    {
        return time.Convert().ToTDT().Convert();
    }
    
    public static Time ToTAI(this Time time)
    {
        return time.Convert().ToTAI().Convert();
    }
    
    public static Time ToGPS(this Time time)
    {
        return time.Convert().ToGPS().Convert();
    }
}