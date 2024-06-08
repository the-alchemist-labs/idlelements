using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class EssenceLab
{
    private static int MAX_LEVEL = 10;
    public static DateTime lastCollectDate { get; private set; }

    public static int incomeLoopSeconds { get { return 5; } }
    public readonly static Dictionary<MapId, BuildingLevel> baseLevelModifiers = new Dictionary<MapId, BuildingLevel>()
    {
        { MapId.MapA, new BuildingLevel(1000, 50, 100) },
        { MapId.MapB, new BuildingLevel(5000, 100, 1000) },
        { MapId.MapC, new BuildingLevel(20000, 500, 5000) },
    };

    static EssenceLab()
    {
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
        return baseLevelModifiers
        .ToList()
        .Sum(kvp => State.Maps.GetMapProgresion(
            kvp.Key).essenceLabLevel * baseLevelModifiers[kvp.Key].BuffBonus
            + baseLevelModifiers[kvp.Key].UnlockBonus);
    }

    public static int GetSecondsSinceLastCollect()
    {
        return (int)(DateTime.Now - lastCollectDate).TotalSeconds;
    }

    public static bool IsMaxLevel()
    {
        return State.Maps.GetCurrentMapProgresion().essenceLabLevel >= MAX_LEVEL;
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
        BuildingLevel essenceLabLevel = baseLevelModifiers[State.Maps.currentMapId];
        return essenceLabLevel.UnlockBonus + map.essenceLabLevel * essenceLabLevel.BuffBonus;
    }

    public static int GetNextLevelBuff()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        BuildingLevel essenceLabLevel = baseLevelModifiers[State.Maps.currentMapId];

        return map.essenceLabLevel == 0 ? essenceLabLevel.UnlockBonus : essenceLabLevel.BuffBonus;
    }

    public static int GetLevelUpCost()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        return (map.essenceLabLevel + 1) * baseLevelModifiers[State.Maps.currentMapId].CostModifier;
    }
}