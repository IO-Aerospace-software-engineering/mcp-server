// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

namespace IO.MCP.Data.SolarSystemObjects;

public enum BarycentersEnum
{
    SOLAR_SYSTEM_BARYCENTER = 0,
    MERCURY_BARYCENTER = 1,
    VENUS_BARYCENTER = 2,
    EARTH_MOON_BARYCENTER = 3,
    MARS_BARYCENTER = 4,
    JUPITER_BARYCENTER = 5,
    SATURN_BARYCENTER = 6,
    URANUS_BARYCENTER = 7,
    NEPTUNE_BARYCENTER = 8,
    PLUTO_BARYCENTER = 9
}

public class Barycenters
{
    public static NaifObject SOLAR_SYSTEM_BARYCENTER = new(0, "SOLAR SYSTEM BARYCENTER", string.Empty);
    public static NaifObject MERCURY_BARYCENTER = new(1, "MERCURY BARYCENTER", string.Empty);
    public static NaifObject VENUS_BARYCENTER = new(2, "VENUS BARYCENTER", string.Empty);
    public static NaifObject EARTH_BARYCENTER = new(3, "EARTH MOON BARYCENTER", string.Empty);
    public static NaifObject MARS_BARYCENTER = new(4, "MARS BARYCENTER", string.Empty);
    public static NaifObject JUPITER_BARYCENTER = new(5, "JUPITER BARYCENTER", string.Empty);
    public static NaifObject SATURN_BARYCENTER = new(6, "SATURN BARYCENTER", string.Empty);
    public static NaifObject URANUS_BARYCENTER = new(7, "URANUS BARYCENTER", string.Empty);
    public static NaifObject NEPTUNE_BARYCENTER = new(8, "NEPTUNE BARYCENTER", string.Empty);
    public static NaifObject PLUTO_BARYCENTER = new(9, "PLUTO BARYCENTER", string.Empty);
}