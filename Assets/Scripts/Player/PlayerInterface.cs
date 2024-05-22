using System;

[Serializable]
public struct PlayerData
{
    public PlayerInfo playerInfo;
}

[Serializable]
public struct PlayerInfo
{
    public string ign;
    public string playerId;
    public int level;
}
