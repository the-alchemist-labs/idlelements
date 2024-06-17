public static class FileName
{
    public const string Maps = "maps";
    public const string Elementals = "elementals";
    public const string State = "state";
}

public static class Tags
{
    public const string MapInfo = "MapInfo";
}

public static class Conts
{
    public const bool IsEncrypted = false;
    public const int MaxIdleSecond = 43200; // 12h
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
    Ice,
    Lightning,
    Chaos
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