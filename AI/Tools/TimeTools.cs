using System.ComponentModel;
using IO.MCP.AI.Models;
using ModelContextProtocol.Server;

namespace IO.MCP.AI.Tools;

[McpServerToolType]
public class TimeTools
{
    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Convert date time to target kind", UseStructuredContent = true)]
    [Description("Converts a date time to a target kind")]
    public static Time ConvertDateTime([Description("Date time to convert")] Time dateTime, [Description("kind of the target")] DateKind kindTarget)
    {
        if (kindTarget == DateKind.UTC)
        {
            return dateTime.ToUTC();
        }

        if (kindTarget == DateKind.TDB)
        {
            return dateTime.ToTDB();
        }

        if (kindTarget == DateKind.TAI)
        {
            return dateTime.ToTAI();
        }

        if (kindTarget == DateKind.TDT)
        {
            return dateTime.ToTDT();
        }

        if (kindTarget == DateKind.GPS)
        {
            return dateTime.ToGPS();
        }

        return dateTime;
    }

    [McpServerTool(Idempotent = true, ReadOnly = true, Title = "Get the current date time in UTC", UseStructuredContent = true)]
    [Description("Get the current date time in UTC")]
    public static Time CurrentDateTime()
    {
        return new Time(DateTime.UtcNow, DateKind.UTC);
    }
}