using System;

[Serializable]
public struct GameData
{
    public PlayerInfo playerInfo;
    public string lastTimestamp;
    public Resources resources;
}

[Serializable]
public struct PlayerInfo
{
    public string ign;
    public string playerId;
    public int level;
}

[Serializable]
public struct Resources
{
    public int gold;
    public int diamonds;
}
