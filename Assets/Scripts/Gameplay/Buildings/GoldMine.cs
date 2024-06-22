using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class GoldMine
{
    public static DateTime lastCollectDate { get; private set; }

    public static int incomeLoopSeconds { get { return 5; } }

    public readonly static Dictionary<MapId, BuildingLevel> mapGoldMine = new Dictionary<MapId, BuildingLevel>()
    {
        { MapId.FireWater, new BuildingLevel(5, 500, 50, 10) },
        { MapId.WaterAir, new BuildingLevel(5, 1500, 100, 20) },
        { MapId.EarthFire, new BuildingLevel(5, 3000, 250, 30)  },
    };

    public static BuildingLevel currentMapGoldMine { get { return mapGoldMine[State.Maps.currentMapId]; } }

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
        return mapGoldMine
        .ToList()
        .Sum(kvp =>
        {
            MapProgression mapProgression = State.Maps.GetMapProgresion(kvp.Key);
            State.Maps.GetMapProgresion(kvp.Key);
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
        return State.Maps.GetCurrentMapProgresion().goldMineLevel >= currentMapGoldMine.MaxLevel;
    }

    public static bool LevelUp()
    {
        if (IsMaxLevel() || State.gold < GetLevelUpCost())
        {
            return false;
        }

        int levelUpCost = GetLevelUpCost();
        State.UpdateGold(-levelUpCost);
        State.Maps.GetCurrentMapProgresion().goldMineLevel++;
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
        return map.goldMineLevel * currentMapGoldMine.CostModifier;
    }

    public static int GetLevelUpBuff()
    {
        return (State.Maps.GetCurrentMapProgresion().goldMineLevel + 1) * currentMapGoldMine.BuffBonus;
    }

    private static int GetTotalBuffByMap(MapId mapId)
    {
        return Enumerable
        .Range(1, State.Maps.GetMapProgresion(mapId).goldMineLevel)
        .Select(i => i * mapGoldMine[mapId].BuffBonus)
        .Sum();
    }
}