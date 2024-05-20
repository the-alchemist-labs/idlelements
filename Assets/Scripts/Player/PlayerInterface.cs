using System;

namespace PlayerInterface
{
    public interface IPlayerData
    {
        PlayerInfo playerInfo { get; set; }
        Inventory inventory { get; set; }
        int nextLevelRequiredExp { get; set; }
        string mapLocation { get; set; }
    }

    [Serializable]
    public class PlayerData : IPlayerData
    {
        public PlayerInfo playerInfo { get; set; }
        public Inventory inventory { get; set; }
        public int nextLevelRequiredExp { get; set; }
        public string mapLocation { get; set; }
    }
    
    [Serializable]
    public struct PlayerInfo
    {
        public string ign;
        public string playerId;
        public int level;
        public int currentLevelExp;
        public BaseStats baseStats;
    }

    [Serializable]
    public struct BaseStats
    {
        public float encounterRate;
        public float catchRate;
    }

    [Serializable]
    public struct PlayerResources
    {
        public int gold;
        public int diamonds;
    }

    [Serializable]
    public struct Inventory
    {
        public PlayerResources resources;

    }
}
