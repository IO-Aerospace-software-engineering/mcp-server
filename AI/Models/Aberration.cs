using System.ComponentModel;

namespace IO.MCP.AI.Models;

public enum Aberration
{
    [Description("NONE")] None,
    [Description("Light time corrected from observer to target")] LT,
    [Description("Light time corrected with stellar aberration from observer to target")] LTS,
    [Description("Converged newtonian from observer to target")] CN,
    [Description("Converged newtonian with stellar aberration from observer to target")] CNS,
    [Description("Light time corrected from target to observer")] XLT,
    [Description("Light time corrected with stellar aberration from target to observer")] XLTS,
    [Description("Converged newtonian from target to observer")] XCN,
    [Description("Converged newtonian with stellar aberration from target to observer")] XCNS,
}