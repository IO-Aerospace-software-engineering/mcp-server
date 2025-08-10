using System.Reflection;

namespace IO.MCP.Data;

public class NaifObjectProvider<T>
{
    internal static IEnumerable<NaifObject> Get()
    {
        return typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).Select(x => x.GetValue(null)).OfType<NaifObject>();
    }
}