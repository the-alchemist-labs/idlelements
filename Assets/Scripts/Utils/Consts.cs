public static class FileName
{
    public const string Maps = "maps";
    public const string Elementals = "elementals";
    public const string State = "state";
}

public static class Tags
{
    public const string MapsPanel = "MapsPanel";
    public const string CatchToastLocation = "CatchToastLocation";
}

public static class PlayerPrefKeys
{
    public const string FirstTimePlaying = "FirstTimePlayingKey";
}

public static class Consts
{
    public const bool IsEncrypted = false;
    public const int MaxIdleSecond = 43200; // 12h
    public const int LevelUpOrbsGain = 200;
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
    None,
    Ferine,
    Ferion,
    Wizo,
    Wizar,
    Bolli,
    Bider,
    Bulldo,
    Seria,
    Galeria,
    Freezion,
    Stromeon,
    Zapeon,
    Volx,
}

public enum MapId
{
    None,
    FireWater,
    WaterAir,
    EarthFire,
    IceEarth,
    LightningAir,
}

public enum BonusResource
{
    Gold,
    Essence,
    EncounterSpeed,
    Experience,
}
