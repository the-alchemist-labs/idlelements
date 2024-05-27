public static class FileName
{
    public const string Maps = "maps";
    public const string Elementals = "elementals";
    public const string State = "state";
    public const string Deck = "deck";
}

public static class Conts
{
    public const bool IsEncrypted = false;
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
}

public enum MapId
{
    MapA = 1,
    MapB,
}