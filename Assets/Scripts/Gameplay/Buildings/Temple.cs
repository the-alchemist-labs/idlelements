using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapTemple : BuildingLevel
{
    public int BoostEffect { get; }
    public int BoostCost { get; }

    public MapTemple(int interval, int costModifier, int buffBonus, int boostEffect, int boostCost, int maxLevel)
         : base(interval, costModifier, buffBonus, maxLevel)
    {
        BoostEffect = boostEffect;
        BoostCost = boostCost;
    }
}

public static class Temple
{
    public readonly static Dictionary<MapId, MapTemple> mapTemples = new Dictionary<MapId, MapTemple>()
    {
        { MapId.FireWater, new MapTemple(300, 100, 5, 10, 10, 10) },
        { MapId.WaterAir, new MapTemple(350, 500, 5, 15, 10, 20) },
        { MapId.EarthFire, new MapTemple(400, 1000, 5, 20, 15, 25) },
    };

    public static MapTemple currentTemple { get { return mapTemples[State.Maps.currentMapId]; } }
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
        float encounterSpeedModifier = currentTemple.Interval * (float)GetTotalBuff() / 100;
        return (int)(currentTemple.Interval - encounterSpeedModifier);
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
        return currentTempleLevel >= mapTemples[State.Maps.currentMapId].MaxLevel;
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
        return mapTemples[State.Maps.currentMapId].BoostCost;
    }

    public static void Boost()
    {
        MapTemple temple = mapTemples[State.Maps.currentMapId];
        State.UpdatelastEncounterDate(State.lastEncounterDate.AddSeconds(-temple.BoostEffect));
        State.UpdateEssence(-temple.BoostCost);
    }

    public static int GetTotalBuff()
    {
        return currentTemple.BuffBonus * currentTempleLevel;
    }

    public static int GetLevelUpCost()
    {
        BuildingLevel templeLevel = mapTemples[State.Maps.currentMapId];
        return (currentTempleLevel + 1) * templeLevel.CostModifier;
    }

    public static int GetLevelUpBuff()
    {
        return (State.Maps.GetCurrentMapProgresion().goldMineLevel + 1) * currentTemple.BuffBonus;
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