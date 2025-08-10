using IO.MCP.AI.Models;

namespace IO.MCP.AI.Converters;

public static class WindowConverter
{
    public static Window Convert(this in Astrodynamics.TimeSystem.Window window)
    {
        return new Window(window.StartDate.Convert(), window.EndDate.Convert());
    }

    public static Astrodynamics.TimeSystem.Window Convert(this Window window)
    {
        return new Astrodynamics.TimeSystem.Window(window.Start.Convert(), window.End.Convert());
    }
    
    public static IEnumerable<Window> Convert(this IEnumerable<Astrodynamics.TimeSystem.Window> windows)
    {
        return windows.Select(x => x.Convert()).ToArray();
    }

    public static IEnumerable<Astrodynamics.TimeSystem.Window> Convert(this IEnumerable<Window> windows)
    {
        return windows.Select(x => x.Convert()).ToArray();
    }
}