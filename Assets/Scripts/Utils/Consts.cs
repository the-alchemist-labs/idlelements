public static class FileName
{
    public const string MapCatalog = "MapCatalog";
    public const string ElementalCatalog = "ElementalCatalog";
    public const string InventoryCatalog = "InventoryCatalog";
    public const string MapManagerState = "MapManagerState";
    public const string ElementalManagerState = "ElementalManagerState";
    public const string PlayerState = "PlayerState";

}

public static class Tags
{
    public const string MapsPanel = "MapsPanel";
    public const string CatchToastLocation = "CatchToastLocation";
    public const string MainManager = "MainManager";
}

public static class PlayerPrefKeys
{
    public const string BGM_Volume = "BGM_Volume";
    public const string SFX_Volume = "SFX_Volume";
    public const string SELECTED_BALL = "SelectedBall";

}

public static class Consts
{
    public const bool IsEncrypted = false;
    public const int MaxIdleSecond = 43200; // 12h
    public const int LevelUpOrbsGain = 200;
    public const string ServerURI = "192.168.1.223:3000";
    public const string DiscordUrl = "https://discord.gg/QQJmPV6s";
}

public static class SceneNames
{
    public const string Loading = "LoadingScene";
    public const string Main = "MainScene";
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

public enum Status
{
    Success,
    Failed
}

public enum Respond
{
    Accept,
    Reject
}