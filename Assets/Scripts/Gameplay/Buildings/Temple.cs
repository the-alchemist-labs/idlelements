using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TempleSpecs : BuildingSpecs
{
    public int BoostEffect { get; }
    public int BoostCost { get; }
    public int Interval { get; }

    public TempleSpecs(int interval, int baseCost, int buffBonus, int boostEffect, int boostCost, int maxLevel)
         : base(baseCost, buffBonus, maxLevel)
    {
        BoostEffect = boostEffect;
        BoostCost = boostCost;
        Interval = interval;
    }
}

public static class Temple
{
    public static TempleSpecs currentTempleSpecs { get { return State.Maps.GetMap(State.Maps.currentMapId).templeSpecs; } }
    public static int currentTempleLevel { get { return State.Maps.currentMapProgression.templeLevel; } }

    public static IEnumerator StartRoutine()
    {
        while (true)
        {
            float elapedsSeconds = GetSecondsSincelastEncounterDate(State.lastEncounterDate);

            if (elapedsSeconds > GetEncounterSpeed())
            {
                ElementalId elementalId = GetEncounter(State.Maps.currentMap.elementalEncounters);
                ElementalCaught(elementalId);
            }

            yield return new WaitForSeconds(1);
        }
    }

    public static int GetEncounterSpeed()
    {
        float encounterSpeedModifier = currentTempleSpecs.Interval * (float)GetTotalSpeedBuff() / 100;
        return (int)(currentTempleSpecs.Interval - encounterSpeedModifier);
    }

    public static int GetSecondsUntilNextEncounter()
    {
        int secondsSincelastEncounterDate = (int)(DateTime.Now - State.lastEncounterDate).TotalSeconds;
        return (GetEncounterSpeed() - secondsSincelastEncounterDate) % GetEncounterSpeed();
    }

    public static int GetSecondsSincelastEncounterDate(DateTime date)
    {
        TimeSpan diff = DateTime.Now - date;
        return Math.Min((int)diff.TotalSeconds, Conts.MaxIdleSecond);
    }

    public static ElementalId GetEncounter(ElementalEncounter[] elementalEncounters)
    {
        float randomValue = (float)new System.Random().NextDouble();
        float cumulativeChance = 0f;

        return elementalEncounters.First(encounter =>
        {
            cumulativeChance += encounter.encounterChance;
            return randomValue <= cumulativeChance;
        }).elementalId;
    }

    public static bool IsMaxLevel()
    {
        return currentTempleLevel >= currentTempleSpecs.MaxLevel;
    }

    public static bool LevelUp()
    {
        if (IsMaxLevel() || State.gold < GetLevelUpCost())
        {
            return false;
        }

        int levelUpCost = GetLevelUpCost();
        State.UpdateGold(-levelUpCost);
        State.Maps.currentMapProgression.TempleLevelUp();
        GameEvents.IdleGainsChanged();
        return true;
    }

    public static int GetBoostCost()
    {
        return currentTempleSpecs.BoostCost;
    }

    public static void Boost()
    {
        State.UpdatelastEncounterDate(State.lastEncounterDate.AddSeconds(-currentTempleSpecs.BoostEffect));
        State.UpdateEssence(-currentTempleSpecs.BoostCost);
    }

    public static int GetTotalSpeedBuff()
    {
        int gain = State.Maps.all
        .Select(map => map.id)
        .Where(mapId => State.Maps.GetMapProgression(mapId).isUnlocked)
        .ToList()
        .Sum(mapId =>
        {
            MapProgression mapProgression = State.Maps.GetMapProgression(mapId);
            return mapProgression != null
            ? GetSpeedBuff(mapProgression.templeLevel, GetBaseSpeedBuffByMap(mapId))
            : 0;
        });

        float partyBonus = State.party.GetPartyBonusMultipier(BonusResource.EncounterSpeed);

        return (int)(gain + (gain * partyBonus));
    }

    public static int GetLevelUpCost()
    {
        int cost = currentTempleLevel * currentTempleSpecs.BaseCost;
        float levelModifierExtraCost = currentTempleLevel * 0.5f;
        return (int)(cost + (cost * levelModifierExtraCost));
    }

    public static int GetLevelUpSpeedBuff()
    {
        return (currentTempleLevel + 1) * currentTempleSpecs.BaseBonus;
    }

    public static int GetCurrentMapSpeedGain()
    {
        return GetSpeedBuff(currentTempleLevel, GetBaseSpeedBuffByMap(State.Maps.currentMapId));
    }

    public static void ElementalCaught(ElementalId elementalId, bool shouldTriggerToast = true)
    {
        Elemental elemental = State.Elementals.GetElemental(elementalId);

        State.Elementals.UpdateElementalTokens(elementalId, 1);
        State.GainExperience(elemental.expGain);
        State.UpdateElementalCaught(elementalId, shouldTriggerToast);

        if (!State.Elementals.IsElementalRegistered(elementalId))
        {
            State.Elementals.MarkElementalAsCaught(elementalId);
            State.UpdateOrbs(elemental.orbsGain);
        }
    }

    private static int GetSpeedBuff(int templeLevel, int baseBonus)
    {
        int levelBonus = (int)((templeLevel - 1) * baseBonus * 0.3f);
        return baseBonus + levelBonus;
    }

    private static int GetBaseSpeedBuffByMap(MapId mapId)
    {
        return State.Maps.GetMap(mapId).templeSpecs.BaseBonus;
    }
}
