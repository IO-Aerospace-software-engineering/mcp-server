namespace IO.MCP.Data;

public readonly record struct NaifObject(int NaifId, string Name, string Frame)
{
    public override string ToString()
    {
        return Name;
    }
}