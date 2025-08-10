using System.ComponentModel.DataAnnotations;

namespace IO.MCP.AI.Models;

public class Window : IEquatable<Window>
{
    public bool Equals(Window other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Nullable.Equals(Start, other.Start) && Nullable.Equals(End, other.End);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Window)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }

    public static bool operator ==(Window left, Window right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Window left, Window right)
    {
        return !Equals(left, right);
    }


    [Required]
    public Time Start { get; set; }


    [Required]
    public Time End { get; set; }

    public TimeSpan Length
    {
        get
        {
            if (End == null || Start == null) return TimeSpan.Zero;
            return End.DateTime - Start.DateTime;
        }
    }

    public Window()
    {
        Start = new Time();
        End = new Time();
    }

    public Window(Time start, Time end)
    {
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}{Environment.NewLine}{nameof(Length)}: {Length}";
    }
}