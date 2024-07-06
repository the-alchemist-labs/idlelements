using System;
using System.Linq;
using System.Threading.Tasks;

public static class EssenceLab
{
    public static DateTime lastCollectDate { get; private set; }
    public static int incomeLoopSeconds { get { return 60; } }

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
            ? CalculateEssenceGain(mapProgression.essenceLabLevel, GetBaseBonusByMap(mapId))
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
        State.Maps.currentMapProgression.EssenceLabLevelUp();
        GameEvents.IdleGainsChanged();

        return true;
    }

    public static int GetEssenceGain()
    {
        return CalculateEssenceGain(currentEssenceLabLevel, GetBaseBonusByMap(State.Maps.currentMapId));
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
        return State.Maps.GetMap(mapId).essenceLabSpecs.BaseBonus;
    }
}