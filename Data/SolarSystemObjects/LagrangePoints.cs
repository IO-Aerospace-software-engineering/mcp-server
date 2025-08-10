// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

namespace IO.MCP.Data.SolarSystemObjects;

public enum LagrangePointsEnum
{
    L1 = 391,
    L2 = 392,
    L4 = 394,
    L5 = 395
}
public class LagrangePoints
{
    public static NaifObject L1 = new(391, "L1", null);
    public static NaifObject L2 = new(392, "L2", null);
    public static NaifObject L4 = new(394, "L4", null);
    public static NaifObject L5 = new(395, "L5", null);
}