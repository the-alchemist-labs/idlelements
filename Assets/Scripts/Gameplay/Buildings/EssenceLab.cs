using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class EssenceLab
{
    private static int MAX_LEVEL = 10;
    public static DateTime lastCollectDate { get; private set; }

    public static int incomeLoopSeconds { get { return 5; } }
    public readonly static Dictionary<MapId, int> baseBonusModifiers = new Dictionary<MapId, int>()
    {
        { MapId.MapA, 10 },
        { MapId.MapB, 50 },
        { MapId.MapC, 100  },
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

    private static int GetTotalEssenceFromAllMaps()
    {
        return baseBonusModifiers
        .ToList()
        .Sum(kvp => State.Maps.GetMapProgresion(kvp.Key).essenceLabLevel * baseBonusModifiers[kvp.Key]);
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

        return map.essenceLabLevel * baseBonusModifiers[State.Maps.currentMapId];
    }

    public static int GetNextLevelBuff()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        int essenceLabLevel = baseBonusModifiers[State.Maps.currentMapId];

        if (map.essenceLabLevel == 0) return baseBonusModifiers[State.Maps.currentMapId];
        return essenceLabLevel;
    }

    public static int GetLevelUpCost()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        return (map.essenceLabLevel + 1) * baseBonusModifiers[State.Maps.currentMapId];
    }
}