using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class GoldMine
{
    private static int MAX_LEVEL = 10;
    public static DateTime lastCollectDate { get; private set; }

    public static int incomeLoopSeconds { get { return 5; } }
    public readonly static Dictionary<MapId, int> baseBonusModifiers = new Dictionary<MapId, int>()
    {
        { MapId.MapA, 100 },
        { MapId.MapB, 500 },
        { MapId.MapC, 1000  },
    };

    static GoldMine()
    {
        _ = Collect();
    }

    private static async Task Collect()
    {
        while (true)
        {
            lastCollectDate = DateTime.Now;
            State.UpdateGold(GetTotalGoldFromAllMaps());
            await Task.Delay(incomeLoopSeconds * 1000);
        }
    }

    private static int GetTotalGoldFromAllMaps()
    {
        return baseBonusModifiers
        .ToList()
        .Sum(kvp => State.Maps.GetMapProgresion(kvp.Key).goldMineLevel * baseBonusModifiers[kvp.Key]);
    }

    public static int GetSecondsSinceLastCollect()
    {
        return (int)(DateTime.Now - lastCollectDate).TotalSeconds;
    }

    public static bool IsMaxLevel()
    {
        return State.Maps.GetCurrentMapProgresion().goldMineLevel >= MAX_LEVEL;
    }

    public static void LevelUp()
    {
        if (IsMaxLevel())
        {
            return;
        }

        int levelUpCost = GetLevelUpCost();
        State.UpdateGold(-levelUpCost);
        State.Maps.GetCurrentMapProgresion().goldMineLevel++;
    }

    public static int GetTotalBuff()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        if (map.goldMineLevel == 0) return 0;

        return map.goldMineLevel * baseBonusModifiers[State.Maps.currentMapId];
    }

    public static int GetNextLevelBuff()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        int goldMineLevel = baseBonusModifiers[State.Maps.currentMapId];

        if (map.goldMineLevel == 0) return baseBonusModifiers[State.Maps.currentMapId];
        return goldMineLevel;
    }

    public static int GetLevelUpCost()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        return (map.goldMineLevel + 1) * baseBonusModifiers[State.Maps.currentMapId];
    }
}