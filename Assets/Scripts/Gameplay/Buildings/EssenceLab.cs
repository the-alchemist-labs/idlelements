using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Analytics;

public static class EssenceLab
{
    public static DateTime lastCollectDate { get; private set; }
    public static int incomeLoopSeconds { get { return 60; } }

    public static BuildingSpecs currentMapEssenceLabSpecs { get { return MapCatalog.Instance.GetMap(MapManager.Instance.currentMapId).essenceLabSpecs; } }
    public static int currentEssenceLabLevel { get { return MapManager.Instance.currentMapProgression.essenceLabLevel; } }

    static EssenceLab()
    {
        _ = Collect();
    }

    private static async Task Collect()
    {
        while (true)
        {
            lastCollectDate = DateTime.Now;
            Player.Instance.Resources.UpdateEssence(GetTotalEssenceFromAllMaps());
            await Task.Delay(incomeLoopSeconds * 1000);
        }
    }

    public static int GetTotalEssenceFromAllMaps()
    {
        int gain = MapCatalog.Instance.maps
        .Select(map => map.id)
        .Where(mapId => Player.Instance.Level >= MapCatalog.Instance.GetMap(mapId).requiredLevel)
        .ToList()
        .Sum(mapId =>
        {
            MapProgression mapProgression = MapManager.Instance.GetMapProgression(mapId);
            return mapProgression != null
            ? CalculateEssenceGain(mapProgression.essenceLabLevel, GetBaseBonusByMap(mapId))
            : 0;
        });

        return gain;
    }

    public static int GetSecondsSinceLastCollect()
    {
        return (int)(DateTime.Now - lastCollectDate).TotalSeconds;
    }

    public static bool IsMaxLevel()
    {
        return currentEssenceLabLevel >= currentMapEssenceLabSpecs.MaxLevel;
    }

    public static bool LevelUp()
    {
        if (IsMaxLevel() || Player.Instance.Resources.Gold < GetLevelUpCost())
        {
            return false;
        }

        int levelUpCost = GetLevelUpCost();
        Player.Instance.Resources.UpdateGold(-levelUpCost);
        MapManager.Instance.currentMapProgression.EssenceLabLevelUp();
        GameEvents.IdleGainsChanged();

        if (IsMaxLevel())
            Analytics.CustomEvent("EssenceLabMaxLevel", new Dictionary<string, object> { { "map", MapManager.Instance.currentMapId } });

        return true;
    }

    public static int GetEssenceGain()
    {
        return CalculateEssenceGain(currentEssenceLabLevel, GetBaseBonusByMap(MapManager.Instance.currentMapId));
    }

    public static int GetLevelUpCost()
    {
        int cost = currentEssenceLabLevel * currentMapEssenceLabSpecs.BaseCost;
        float levelModifierExtraCost = (currentEssenceLabLevel - 1) * 0.1f;
        return (int)(cost + (cost * levelModifierExtraCost));
    }

    public static int GetLevelUpGains()
    {
        return (currentEssenceLabLevel + 1) * currentMapEssenceLabSpecs.BaseBonus;
    }

    private static int CalculateEssenceGain(int essenceLabLevel, int baseBonus)
    {
        int levelBonus = (int)((essenceLabLevel - 1) * baseBonus * 0.3f);
        return baseBonus + levelBonus;
    }

    private static int GetBaseBonusByMap(MapId mapId)
    {
        return MapCatalog.Instance.GetMap(mapId).essenceLabSpecs.BaseBonus;
    }
}