using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public class TempleSpecs : BuildingSpecs
{
    public int BoostEffect { get; }
    public int BoostCost { get; }
    public int Interval { get; }

    public TempleSpecs(int interval, int baseCost, int baseBonus, int boostEffect, int boostCost, int maxLevel)
         : base(baseCost, baseBonus, maxLevel)
    {
        BoostEffect = boostEffect;
        BoostCost = boostCost;
        Interval = interval;
    }
}

public static class Temple
{
    public static TempleSpecs currentTempleSpecs { get { return MapCatalog.Instance.GetMap(MapManager.Instance.currentMapId).templeSpecs; } }
    public static int currentTempleLevel { get { return MapManager.Instance.currentMapProgression.templeLevel; } }

    public static IEnumerator StartRoutine()
    {
        while (true)
        {
            float elapedsSeconds = GetSecondsSincelastEncounterDate(ElementalManager.Instance.lastEncounterDate);

            if (elapedsSeconds > GetEncounterSpeed())
            {
                ElementalId elementalId = GetEncounter(MapManager.Instance.currentMap.elementalEncounters);
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
        int secondsSincelastEncounterDate = (int)(DateTime.Now - ElementalManager.Instance.lastEncounterDate).TotalSeconds;
        return (GetEncounterSpeed() - secondsSincelastEncounterDate) % GetEncounterSpeed();
    }

    public static int GetSecondsSincelastEncounterDate(DateTime date)
    {
        TimeSpan diff = DateTime.Now - date;
        return Math.Min((int)diff.TotalSeconds, Consts.MaxIdleSecond);
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
        if (IsMaxLevel() || Player.Instance.Resources.Gold < GetLevelUpCost())
        {
            return false;
        }

        int levelUpCost = GetLevelUpCost();
        Player.Instance.Resources.UpdateGold(-levelUpCost);
        MapManager.Instance.currentMapProgression.TempleLevelUp();
        GameEvents.IdleGainsChanged();

        if (IsMaxLevel())
            Analytics.CustomEvent("GoldMineMaxLevel", new Dictionary<string, object> { { "map", MapManager.Instance.currentMapId } });

        return true;
    }

    public static int GetBoostCost()
    {
        return currentTempleSpecs.BoostCost;
    }

    public static void Boost()
    {
        ElementalManager.Instance.UpdatelastEncounterDate(ElementalManager.Instance.lastEncounterDate.AddSeconds(-currentTempleSpecs.BoostEffect));
        Player.Instance.Resources.UpdateEssence(-currentTempleSpecs.BoostCost);
    }

    public static int GetTotalSpeedBuff()
    {
        int gain = MapCatalog.Instance.maps
        .Select(map => map.id)
        .Where(mapId => Player.Instance.Level >= MapCatalog.Instance.GetMap(mapId).requiredLevel)
        .ToList()
        .Sum(mapId =>
        {
            MapProgression mapProgression = MapManager.Instance.GetMapProgression(mapId);
            return mapProgression != null
            ? GetSpeedBuff(mapProgression.templeLevel, GetBaseSpeedBuffByMap(mapId))
            : 0;
        });

        float partyBonus = Player.Instance.Party.GetPartyBonusMultipier(BonusResource.EncounterSpeed);

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
        return GetSpeedBuff(currentTempleLevel, GetBaseSpeedBuffByMap(MapManager.Instance.currentMapId));
    }

    public static void ElementalCaught(ElementalId elementalId, bool isNaturalEncounter = true)
    {
        Elemental elemental = ElementalCatalog.Instance.GetElemental(elementalId);

        ElementalManager.Instance.UpdateElementalTokens(elementalId, 1);
        Player.Instance.GainExperience(elemental.expGain);
        ElementalManager.Instance.UpdateElementalCaught(elementalId, isNaturalEncounter);

        if (!ElementalManager.Instance.IsElementalRegistered(elementalId))
        {
            ElementalManager.Instance.MarkElementalAsCaught(elementalId);
            Player.Instance.Resources.UpdateOrbs(elemental.orbsGain);
            Analytics.CustomEvent("ElementalCaught", new Dictionary<string, object> { { "id", elementalId } });
        }
    }

    private static int GetSpeedBuff(int templeLevel, int baseBonus)
    {
        int levelBonus = (int)((templeLevel - 1) * baseBonus * 0.3f);
        return baseBonus + levelBonus;
    }

    private static int GetBaseSpeedBuffByMap(MapId mapId)
    {
        return MapCatalog.Instance.GetMap(mapId).templeSpecs.BaseBonus;
    }
}
