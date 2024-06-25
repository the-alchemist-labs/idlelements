using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class EssenceLab
{
    public static DateTime lastCollectDate { get; private set; }
    public static int incomeLoopSeconds { get { return 5; } }
    public readonly static Dictionary<MapId, BuildingLevel> mapEssenceLab = new Dictionary<MapId, BuildingLevel>()
    {
        { MapId.FireWater, new BuildingLevel(3, 1000, 50, 10) },
        { MapId.WaterAir, new BuildingLevel(3, 5000, 100, 20) },
        { MapId.EarthFire, new BuildingLevel(3, 20000, 300, 30) },
    };

    public static BuildingLevel currentMapEssenceLab { get { return mapEssenceLab[State.Maps.currentMapId]; } }

    static EssenceLab()
    {
        // TODO: register to map change and set the mapEssenceLab of the relevant map
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
        return mapEssenceLab
        .ToList()
        .Sum(kvp =>
        {
            MapProgression mapProgression = State.Maps.GetMapProgression(kvp.Key);
            State.Maps.GetMapProgression(kvp.Key);
            return mapProgression != null
            ? GetTotalBuffByMap(kvp.Key)
            : 0;
        });
    }

    public static int GetSecondsSinceLastCollect()
    {
        return (int)(DateTime.Now - lastCollectDate).TotalSeconds;
    }

    public static bool IsMaxLevel()
    {
        return State.Maps.GetCurrentMapProgresion().essenceLabLevel >= mapEssenceLab[State.Maps.currentMapId].MaxLevel;
    }

    public static bool LevelUp()
    {
        if (IsMaxLevel() || State.gold < GetLevelUpCost())
        {
            return false;
        }

        int levelUpCost = GetLevelUpCost();
        State.UpdateGold(-levelUpCost);
        State.Maps.GetCurrentMapProgresion().essenceLabLevel++;
        GameEvents.IdleGainsChanged();

        return true;
    }

    public static int GetTotalBuff()
    {
        return GetTotalBuffByMap(State.Maps.currentMapId);
    }

    public static int GetLevelUpCost()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        return (map.essenceLabLevel + 1) * mapEssenceLab[State.Maps.currentMapId].CostModifier;
    }

    public static int GetLevelUpBuff()
    {
        return (State.Maps.GetCurrentMapProgresion().essenceLabLevel + 1) * currentMapEssenceLab.BuffBonus;
    }

    private static int GetTotalBuffByMap(MapId mapId)
    {
        return Enumerable
        .Range(1, State.Maps.GetMapProgression(mapId).essenceLabLevel)
        .Select(i => i * mapEssenceLab[mapId].BuffBonus)
        .Sum();
    }
}