using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class EssenceLab
{
    public static DateTime lastCollectDate { get; private set; }
    public static int incomeLoopSeconds { get { return 5; } }
    public readonly static Dictionary<MapId, BuildingLevel> buildingInfo = new Dictionary<MapId, BuildingLevel>()
    {
        { MapId.FireWater, new BuildingLevel(1000, 50, 10) },
        { MapId.WaterAir, new BuildingLevel(5000, 100, 20) },
        { MapId.EarthFire, new BuildingLevel(20000, 300, 30) },
    };

    static EssenceLab()
    {
        // TODO: register to map change and set the buildingInfo of the relevant map
        _ = Collect();
    }

    private static async Task Collect()
    {
        while (true)
        {
            lastCollectDate = DateTime.Now;
            State.UpdateEssence(GetTotalEssenceFromAllMaps());
            await Task.Delay(incomeLoopSeconds * 1000);
        }
    }

    public static int GetTotalEssenceFromAllMaps()
    {
        return buildingInfo
        .ToList()
        .Sum(kvp =>
        {
            MapProgression mapProgression = State.Maps.GetMapProgresion(kvp.Key);
            State.Maps.GetMapProgresion(kvp.Key);
            return mapProgression != null
            ? mapProgression.essenceLabLevel * buildingInfo[kvp.Key].BuffBonus
            : 0;
        });
    }

    public static int GetSecondsSinceLastCollect()
    {
        return (int)(DateTime.Now - lastCollectDate).TotalSeconds;
    }

    public static bool IsMaxLevel()
    {
        return State.Maps.GetCurrentMapProgresion().essenceLabLevel >= buildingInfo[State.Maps.currentMapId].MaxLevel;
    }

    public static void LevelUp()
    {
        if (IsMaxLevel())
        {
            return;
        }

        int levelUpCost = GetLevelUpCost();
        State.UpdateGold(-levelUpCost);
        State.Maps.GetCurrentMapProgresion().essenceLabLevel++;
    }

    public static int GetTotalBuff()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        if (map.essenceLabLevel == 0) return 0;
        BuildingLevel essenceLabLevel = buildingInfo[State.Maps.currentMapId];
        return map.essenceLabLevel * essenceLabLevel.BuffBonus;
    }

    public static int GetLevelUpCost()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        return (map.essenceLabLevel + 1) * buildingInfo[State.Maps.currentMapId].CostModifier;
    }
}