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
    Ferine = 1,
    Ferion,
    Wizo,
    Wizar,
    Bolli,
    Bulldo,
    Seria
}

public enum MapId
{
    FireWater = 1,
    WaterAir,
    EarthFire
}