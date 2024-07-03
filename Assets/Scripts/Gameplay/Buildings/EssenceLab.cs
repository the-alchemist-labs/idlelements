using System;
using System.Linq;
using System.Threading.Tasks;

public static class EssenceLab
{
    public static DateTime lastCollectDate { get; private set; }
    public static int incomeLoopSeconds { get { return 5; } }

    public static BuildingSpecs currentMapEssenceLabSpecs { get { return State.Maps.GetMap(State.Maps.currentMapId).essenceLabSpecs; } }
    public static int currentEssenceLabLevel { get { return State.Maps.currentMapProgression.essenceLabLevel; } }

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

        float partyBonus = State.party.GetPartyBonusMultipier(BonusResource.Essence);

        return (int)(gain + (gain * partyBonus));
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
        return (map.essenceLabLevel + 1) * currentMapEssenceLabSpecs.CostModifier;
    }

    public static int GetLevelUpBuff()
    {
        return (currentEssenceLabLevel + 1) * currentMapEssenceLabSpecs.BuffBonus;
    }

    private static int GetTotalBuffByMap(MapId mapId)
    {
        return Enumerable
        .Range(1, State.Maps.GetMapProgression(mapId).essenceLabLevel)
        .Select(i => i * currentMapEssenceLabSpecs.BuffBonus)
        .Sum();
    }
}