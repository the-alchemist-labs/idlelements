using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class GoldMine
{
    private static int MAX_LEVEL = 10;
    public static DateTime lastCollectDate { get; private set; }

    public static int incomeLoopSeconds { get { return 5; } }
    public readonly static Dictionary<MapId, BuildingLevel> baseLevelModifiers = new Dictionary<MapId, BuildingLevel>()
    {
        { MapId.MapA, new BuildingLevel(500, 1000, 0 ) },
        { MapId.MapB, new BuildingLevel(1500, 5000, 0 ) },
        { MapId.MapC, new BuildingLevel(3000, 8500, 0 )  },
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
        return baseLevelModifiers
        .ToList()
        .Sum(kvp => State.Maps.GetMapProgresion(
            kvp.Key).goldMineLevel * baseLevelModifiers[kvp.Key].BuffBonus
            + baseLevelModifiers[kvp.Key].UnlockBonus);
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

        BuildingLevel goldMineLevel = baseLevelModifiers[State.Maps.currentMapId];
        return map.goldMineLevel * goldMineLevel.BuffBonus + goldMineLevel.UnlockBonus;
    }

    public static int GetNextLevelBuff()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        BuildingLevel goldMineLevel = baseLevelModifiers[State.Maps.currentMapId];

        return map.goldMineLevel == 0 ? goldMineLevel.UnlockBonus : goldMineLevel.BuffBonus;
    }

    public static int GetLevelUpCost()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        return (map.goldMineLevel + 1) * baseLevelModifiers[State.Maps.currentMapId].CostModifier;
    }
}