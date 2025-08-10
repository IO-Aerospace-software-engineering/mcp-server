// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

namespace IO.MCP.Data.SolarSystemObjects;

public enum AsteroidsEnum
{
    CERES = 2000001,
    PALLAS = 2000002,
    VESTA = 2000004,
    PSYCHE = 2000016,
    LUTETIA = 2000021,
    AST_52_EUROPA = 2000052,
    KLEOPATRA = 2000216,
    MATHILDE = 2000253,
    EROS = 2000433,
    DAVIDA = 2000511,
    STEINS = 2002867,
    WILSON_HARRINGTON = 2004015,
    TOUTATIS = 2004179,
    AST_1992KD = 2009969,
    BRAILLE = 2009969,
    ITOKAWA = 2025143,
    BENNU = 2101955,
    RYUGU = 2162173,
    IDA = 2431010,
    DACTYL = 2431011,
    ARROKOTH = 2486958,
    GASPRA = 9511010,
    PATROCLUS_BARYCENTER = 20000617,
    EURYBATES_BARYCENTER = 20003548,
    LEUCUS = 20011351,
    POLYMELE = 20015094,
    ORUS = 20021900,
    DONALDJOHANSON = 20052246,
    DIDYMOS_BARYCENTER = 20065803,
    MENOETIUS = 120000617,
    QUETA = 120003548,
    DIMORPHOS = 120065803,
    PATROCLUS = 920000617,
    EURYBATES = 920003548,
    DIDYMOS = 920065803
}

public class Asteroids
{
    public static NaifObject CERES = new(2000001, "CERES", string.Empty);
    public static NaifObject PALLAS = new(2000002, "PALLAS", string.Empty);
    public static NaifObject VESTA = new(2000004, "VESTA", string.Empty);
    public static NaifObject PSYCHE = new(2000016, "PSYCHE", string.Empty);
    public static NaifObject LUTETIA = new(2000021, "LUTETIA", string.Empty);
    public static NaifObject _52_EUROPA = new(2000052, "AST_52_EUROPA", string.Empty);
    public static NaifObject KLEOPATRA = new(2000216, "KLEOPATRA", string.Empty);
    public static NaifObject MATHILDE = new(2000253, "MATHILDE", string.Empty);
    public static NaifObject EROS = new(2000433, "EROS", string.Empty);
    public static NaifObject DAVIDA = new(2000511, "DAVIDA", string.Empty);
    public static NaifObject STEINS = new(2002867, "STEINS", string.Empty);
    public static NaifObject WILSON_HARRINGTON = new(2004015, "WILSON_HARRINGTON", string.Empty);
    public static NaifObject TOUTATIS = new(2004179, "TOUTATIS", string.Empty);
    public static NaifObject _1992KD = new(2009969, "AST_1992KD", string.Empty);
    public static NaifObject BRAILLE = new(2009969, "BRAILLE", string.Empty);
    public static NaifObject ITOKAWA = new(2025143, "ITOKAWA", string.Empty);
    public static NaifObject BENNU = new(2101955, "BENNU", string.Empty);
    public static NaifObject RYUGU = new(2162173, "RYUGU", string.Empty);
    public static NaifObject IDA = new(2431010, "IDA", string.Empty);
    public static NaifObject DACTYL = new(2431011, "DACTYL", string.Empty);
    public static NaifObject ARROKOTH = new(2486958, "ARROKOTH", string.Empty);
    public static NaifObject GASPRA = new(9511010, "GASPRA", string.Empty);
    public static NaifObject PATROCLUS_BARYCENTER = new(20000617, "PATROCLUS_BARYCENTER", string.Empty);
    public static NaifObject EURYBATES_BARYCENTER = new(20003548, "EURYBATES_BARYCENTER", string.Empty);
    public static NaifObject LEUCUS = new(20011351, "LEUCUS", string.Empty);
    public static NaifObject POLYMELE = new(20015094, "POLYMELE", string.Empty);
    public static NaifObject ORUS = new(20021900, "ORUS", string.Empty);
    public static NaifObject DONALDJOHANSON = new(20052246, "DONALDJOHANSON", string.Empty);
    public static NaifObject DIDYMOS_BARYCENTER = new(20065803, "DIDYMOS_BARYCENTER", string.Empty);
    public static NaifObject MENOETIUS = new(120000617, "MENOETIUS", string.Empty);
    public static NaifObject QUETA = new(120003548, "QUETA", string.Empty);
    public static NaifObject DIMORPHOS = new(120065803, "DIMORPHOS", string.Empty);
    public static NaifObject PATROCLUS = new(920000617, "PATROCLUS", string.Empty);
    public static NaifObject EURYBATES = new(920003548, "EURYBATES", string.Empty);
    public static NaifObject DIDYMOS = new(920065803, "DIDYMOS", string.Empty);
}