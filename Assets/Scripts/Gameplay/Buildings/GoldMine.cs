using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Analytics;

public static class GoldMine
{
    public static DateTime lastCollectDate { get; private set; }

    public static int incomeLoopSeconds { get { return 60; } }

    public static BuildingSpecs currentGoldMineSpecs { get { return MapCatalog.Instance.GetMap(MapManager.Instance.currentMapId).goldMineSpecs; } }
    public static int currentGoldMineLevel { get { return MapManager.Instance.currentMapProgression.goldMineLevel; } }

    static GoldMine()
    {
        _ = Collect();
    }

    private static async Task Collect()
    {
        while (true)
        {
            lastCollectDate = DateTime.Now;
            Player.Instance.Resources.UpdateGold(GetTotalGoldGains());
            await Task.Delay(incomeLoopSeconds * 1000);
        }
    }

    public static int GetTotalGoldGains()
    {
        int gain = MapCatalog.Instance.maps
        .Select(map => map.id)
        .Where(mapId => Player.Instance.Level >= MapCatalog.Instance.GetMap(mapId).requiredLevel)
        .ToList()
        .Sum(mapId =>
        {
            MapProgression mapProgression = MapManager.Instance.GetMapProgression(mapId);
            return mapProgression != null
            ? CalculateGoldGain(mapProgression.goldMineLevel, GetBaseBonusByMap(mapId))
            : 0;
        });

        return Mathf.CeilToInt(gain);
    }

    public static int GetSecondsSinceLastCollect()
    {
        return (int)(DateTime.Now - lastCollectDate).TotalSeconds;
    }

    public static bool IsMaxLevel()
    {
        return currentGoldMineLevel >= currentGoldMineSpecs.MaxLevel;
    }

    public static bool LevelUp()
    {
        if (IsMaxLevel() || Player.Instance.Resources.Gold < GetLevelUpCost())
        {
            return false;
        }

        int levelUpCost = GetLevelUpCost();
        Player.Instance.Resources.UpdateGold(-levelUpCost);
        MapManager.Instance.currentMapProgression.GoldMineLevelUp();
        GameEvents.IdleGainsChanged();

        if (IsMaxLevel())
            Analytics.CustomEvent("GoldMineMaxLevel", new Dictionary<string, object> { { "map", MapManager.Instance.currentMapId } });

        return true;
    }

    public static int GetGoldGain()
    {
        return CalculateGoldGain(currentGoldMineLevel, GetBaseBonusByMap(MapManager.Instance.currentMapId));
    }

    public static int GetLevelUpCost()
    {
        int cost = currentGoldMineLevel * currentGoldMineSpecs.BaseCost;
        float levelModifierExtraCost = (currentGoldMineLevel - 1) * 0.05f;
        return (int)(cost + (cost * levelModifierExtraCost));
    }

    public static int GetLevelUpGains()
    {
        return CalculateGoldGain(currentGoldMineLevel + 1, GetBaseBonusByMap(MapManager.Instance.currentMapId));
    }

    private static int CalculateGoldGain(int goldMineLevel, int baseBonus)
    {
        int levelBonus = (int)((goldMineLevel - 1) * baseBonus * 0.35f);
        return baseBonus + levelBonus;
    }

    private static int GetBaseBonusByMap(MapId mapId)
    {
        return MapCatalog.Instance.GetMap(mapId).goldMineSpecs.BaseBonus;
    }
}