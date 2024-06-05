public static class FileName
{
    public const string Maps = "maps";
    public const string Elementals = "elementals";
    public const string State = "state";
}

public static class Tags
{
    public const string MapsPanel = "MapsPanel";
    public const string MapData = "MapData";
}

public static class Conts
{
    public const bool IsEncrypted = false;
    public const int MaxIdleSecond = 84000; // 24h
}

public static class SceneNames
{
    public const string IdleMap = "IdleMapScene";
    public const string Main = "MainScene";
}


public enum ElementType
{
    Fire,
    Water,
    Earth,
    Air,
}

public enum ElementalId
{
    ElementalA = 1,
    ElementalB,
    ElementalC,
    ElementalD,
    ElementalE,
    ElementalF,
    ElementalG,
    ElementalH,
    ElementalI,
    ElementalJ,
    ElementalK,
}

public enum MapId
{
    MapA = 1,
    MapB,
    MapC,
}