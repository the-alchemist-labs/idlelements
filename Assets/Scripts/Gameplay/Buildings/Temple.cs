using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapTemple : BuildingLevel
{
    public int BoostEffect { get; }
    public int BoostCost { get; }

   public MapTemple(int costModifier, int buffBonus, int boostEffect, int boostCost, int maxLevel)
        : base(costModifier, buffBonus, maxLevel)
    {
        BoostEffect = boostEffect;
        BoostCost = boostCost;
    }
}

public static class Temple
{
    private static int baseEncounterSpeed = 300;

    public readonly static Dictionary<MapId, MapTemple> mapTemples = new Dictionary<MapId, MapTemple>()
    {
        { MapId.FireWater, new MapTemple(100, 10, 10, 10, 10) },
        { MapId.WaterAir, new MapTemple(500, 10, 15, 10, 20) },
        { MapId.EarthFire, new MapTemple(1000, 10, 20, 15, 25) },
    };


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
        float encounterSpeedModifier = GetTotalBuff() / 100;
        return (int)(baseEncounterSpeed / (1 + encounterSpeedModifier));
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
        return State.Maps.GetCurrentMapProgresion().templeLevel >= mapTemples[State.Maps.currentMapId].MaxLevel;
    }

    public static void LevelUp()
    {
        if (IsMaxLevel())
        {
            return;
        }

        int levelUpCost = GetLevelUpCost();
        State.UpdateGold(-levelUpCost);
        State.Maps.GetCurrentMapProgresion().templeLevel++;
    }

    public static int GetBoostEffect()
    {
        return mapTemples[State.Maps.currentMapId].BoostEffect;
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
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        if (map.templeLevel == 0) return 0;

        return map.templeLevel * mapTemples[State.Maps.currentMapId].BuffBonus;
    }

    public static int GetLevelUpCost()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        BuildingLevel templeLevel = mapTemples[State.Maps.currentMapId];
        return (map.templeLevel + 1) * templeLevel.CostModifier;
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