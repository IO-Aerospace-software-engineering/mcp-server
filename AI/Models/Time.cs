using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace IO.MCP.AI.Models;

[DataContract]
public class Time
{
    [Description("The date and time in the specified kind.")]
    [DataMember(Name = "dateTime",Order = 1)]
    [Required]
    public DateTime DateTime { get; set; }

    [Description("The kind of the date time, e.g. UTC, TDB, TAI, TDT, GPS, Local.")]
    [DataMember(Name = "kind",Order = 2)]
    [Required] 
    public DateKind Kind { get; set; }

    public Time()
    {
    }

    public Time(in DateTime dateTime, in DateKind kind)
    {
        DateTime = dateTime;
        Kind = kind;
    }
}

