using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class GoldMine
{
    public static DateTime lastCollectDate { get; private set; }

    public static int incomeLoopSeconds { get { return 5; } }
    public readonly static Dictionary<MapId, BuildingLevel> buildingInfo = new Dictionary<MapId, BuildingLevel>()
    {
        { MapId.FireWater, new BuildingLevel(500, 50, 10) },
        { MapId.WaterAir, new BuildingLevel(1500, 100, 20) },
        { MapId.EarthFire, new BuildingLevel(3000, 250, 30)  },
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

    public static int GetTotalGoldFromAllMaps()
    {
        return buildingInfo
        .ToList()
        .Sum(kvp =>
        {
            MapProgression mapProgression = State.Maps.GetMapProgresion(kvp.Key);
            return mapProgression != null
            ? mapProgression.goldMineLevel * buildingInfo[kvp.Key].BuffBonus
            : 0;
        });
    }

    public static int GetSecondsSinceLastCollect()
    {
        return (int)(DateTime.Now - lastCollectDate).TotalSeconds;
    }

    public static bool IsMaxLevel()
    {
        return State.Maps.GetCurrentMapProgresion().goldMineLevel >= buildingInfo[State.Maps.currentMapId].MaxLevel;
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

        BuildingLevel goldMineLevel = buildingInfo[State.Maps.currentMapId];
        return map.goldMineLevel * goldMineLevel.BuffBonus;
    }

    public static int GetLevelUpCost()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        return (map.goldMineLevel + 1) * buildingInfo[State.Maps.currentMapId].CostModifier;
    }
}