using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TempleSpecs : BuildingSpecs
{
    public int BoostEffect { get; }
    public int BoostCost { get; }
    public int Interval { get; }

    public TempleSpecs(int interval, int costModifier, int buffBonus, int boostEffect, int boostCost, int maxLevel)
         : base(costModifier, buffBonus, maxLevel)
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
                State.UpdatelastEncounterDate(DateTime.Now);
                Debug.Log("gottcha");
            }

            yield return new WaitForSeconds(1);
        }
    }

    public static int GetEncounterSpeed()
    {
        float encounterSpeedModifier = currentTempleSpecs.Interval * (float)GetTotalBuff() / 100;
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
        State.Maps.GetCurrentMapProgresion().templeLevel++;
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

    public static int GetTotalBuff()
    {
        return currentTempleSpecs.BuffBonus * currentTempleLevel;
    }

    public static int GetLevelUpCost()
    {
        return (currentTempleLevel + 1) * currentTempleSpecs.CostModifier;
    }

    public static int GetLevelUpBuff()
    {
        return (currentTempleLevel + 1) * currentTempleSpecs.BuffBonus;
    }

    private static void ElementalCaught(ElementalId elementalId)
    {
        Elemental elemental = State.Elementals.GetElement(elementalId);

        State.UpdateLastCatch(elementalId);
        State.GainExperience(elemental.expGain);
        State.Elementals.UpdateElementalTokens(elementalId, 1);

        if (!State.Elementals.IsElementalRegistered(elementalId))
        {
            State.Elementals.MarkElementalAsCaught(elementalId);
            State.UpdateOrbs(elemental.orbsGain);
        }
    }
}