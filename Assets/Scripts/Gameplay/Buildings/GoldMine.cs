using System;
using System.Linq;
using System.Threading.Tasks;

public static class GoldMine
{
    public static DateTime lastCollectDate { get; private set; }

    public static int incomeLoopSeconds { get { return 5; } }

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
            State.UpdateGold(GetTotalGoldFromAllMaps());
            await Task.Delay(incomeLoopSeconds * 1000);
        }
    }

    public static int GetTotalGoldFromAllMaps()
    {
        int gain = State.Maps.all
        .Select(map => map.id)
        .Where(mapId => State.Maps.GetMapProgression(mapId).isUnlocked)
        .ToList()
        .Sum(mapId =>
        {
            MapProgression mapProgression = State.Maps.GetMapProgression(mapId);
            return mapProgression != null
            ? GetTotalBuffByMap(mapId)
            : 0;
        });

        float partyBonus = State.party.GetPartyBonusMultipier(BonusResource.Gold);

        return (int)(gain + (gain * partyBonus));
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
        return currentGoldMineLevel * currentGoldMineSpecs.CostModifier;
    }

    public static int GetLevelUpBuff()
    {
        return (currentGoldMineLevel + 1) * currentGoldMineSpecs.BuffBonus;
    }

    private static int GetTotalBuffByMap(MapId mapId)
    {
        return Enumerable
        .Range(1, State.Maps.GetMapProgression(mapId).goldMineLevel)
        .Select(i => i * currentGoldMineSpecs.BuffBonus)
        .Sum();
    }
}