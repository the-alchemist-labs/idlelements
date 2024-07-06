using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static class GoldMine
{
    public static DateTime lastCollectDate { get; private set; }

    public static int incomeLoopSeconds { get { return 60; } }

    public static BuildingSpecs currentGoldMineSpecs { get { return State.Maps.GetMap(State.Maps.currentMapId).goldMineSpecs; } }
    public static int currentGoldMineLevel { get { return State.Maps.currentMapProgression.goldMineLevel; } }

    static GoldMine()
    {
        _ = Collect();
    }

    private static async Task Collect()
    {
        while (true)
        {
            lastCollectDate = DateTime.Now;
            State.UpdateGold(GetTotalGoldGains());
            await Task.Delay(incomeLoopSeconds * 1000);
        }
    }

    public static int GetTotalGoldGains()
    {
        int gain = State.Maps.all
        .Select(map => map.id)
        .Where(mapId => State.Maps.GetMapProgression(mapId).isUnlocked)
        .ToList()
        .Sum(mapId =>
        {
            MapProgression mapProgression = State.Maps.GetMapProgression(mapId);
            return mapProgression != null
            ? CalculateGoldGain(mapProgression.goldMineLevel, GetBaseBonusByMap(mapId))
            : 0;
        });

        float partyBonus = State.party.GetPartyBonusMultipier(BonusResource.Gold);

        return Mathf.CeilToInt(gain + (gain * partyBonus));
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
        if (IsMaxLevel() || State.gold < GetLevelUpCost())
        {
            return false;
        }

        int levelUpCost = GetLevelUpCost();
        State.UpdateGold(-levelUpCost);
        State.Maps.currentMapProgression.GoldMineLevelUp();
        GameEvents.IdleGainsChanged();
        return true;
    }

    public static int GetGoldGain()
    {
        return CalculateGoldGain(currentGoldMineLevel, GetBaseBonusByMap(State.Maps.currentMapId));
    }

    public static int GetLevelUpCost()
    {
        int cost = currentGoldMineLevel * currentGoldMineSpecs.BaseCost;
        float levelModifierExtraCost = (currentGoldMineLevel - 1) * 0.05f;
        return (int)(cost + (cost * levelModifierExtraCost));
    }

    public static int GetLevelUpGains()
    {
        return CalculateGoldGain(currentGoldMineLevel + 1, GetBaseBonusByMap(State.Maps.currentMapId));
    }

    private static int CalculateGoldGain(int goldMineLevel, int baseBonus)
    {
        int levelBonus = (int)((goldMineLevel - 1) * baseBonus  * 0.35f);
        return baseBonus + levelBonus;
    }

    private static int GetBaseBonusByMap(MapId mapId)
    {
        return State.Maps.GetMap(mapId).goldMineSpecs.BaseBonus;
    }
}