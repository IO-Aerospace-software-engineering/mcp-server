// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

namespace IO.MCP.Data.SolarSystemObjects;

public enum InertialFramesEnum
{
    ICRF = 1,
    B1950 = 2,
    FK4 = 3,
    DE_118 = 4,
    DE_96 = 5,
    DE_102 = 6,
    DE_108 = 7,
    DE_111 = 8,
    DE_114 = 9,
    DE_122 = 10,
    DE_125 = 11,
    DE_130 = 12,
    Galactic = 13,
    DE_200 = 14,
    DE_202 = 15,
    MARSIAU = 16,
    ECLIPJ2000 = 17,
    ECLIPB1950 = 18,
    DE_140 = 19,
    DE_142 = 20,
    DE_143 = 21,
    TEME = 1400000
}
public class InertialFrames
{
    public static NaifObject ICRF = new(1, "ICRF", "J2000");
    public static NaifObject B1950 = new(2, "B 1950", "B1950");
    public static NaifObject FK4 = new(3, "FK 4", "FK4");
    public static NaifObject DE118 = new(4, "DE 118", "DE-118");
    public static NaifObject DE96 = new(5, "DE 96", "DE-96");
    public static NaifObject DE102 = new(6, "DE 102", "DE-102");
    public static NaifObject DE108 = new(7, "DE 108", "DE-108");
    public static NaifObject DE111 = new(8, "DE 111", "DE-111");
    public static NaifObject DE114 = new(9, "DE 114", "DE-114");
    public static NaifObject DE122 = new(10, "DE 122", "DE-122");
    public static NaifObject DE125 = new(11, "DE 125", "DE-125");
    public static NaifObject DE130 = new(12, "DE 130", "DE-130");
    public static NaifObject Galactic = new(13, "Galactic", "GALACTIC");
    public static NaifObject DE200 = new(14, "DE 200", "DE-200");
    public static NaifObject DE202 = new(15, "DE 202", "DE-202");
    public static NaifObject MARSIAU = new(16, "Mars IAU", "MARSIAU");
    public static NaifObject ECLIPJ2000 = new(17, "Ecliptic J2000", "ECLIPJ2000");
    public static NaifObject ECLIPB1950 = new(18, "Ecliptic B1950", "ECLIPB1950");
    public static NaifObject DE140 = new(19, "DE 140", "DE-140");
    public static NaifObject DE142 = new(20, "DE 142", "DE-142");
    public static NaifObject DE143 = new(21, "DE 143", "DE-143");
    public static NaifObject TEME = new( 1400000, "TEME", "TEME");

}