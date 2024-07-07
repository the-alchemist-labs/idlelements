using System;
using System.Collections.Generic;
using UnityEngine;

public static class State
{
    private static int MAX_LEVEL = 30;
    public static DateTime lastEncounterDate { get; private set; }

    public static int level { get; private set; }
    public static int experience { get; private set; }
    public static int essence { get; private set; }
    public static int gold { get; private set; }
    public static int orbs { get; private set; }

    public static Party party { get; }
    public static ElementalId lastCaught { get; private set; }
    public static ElementalsData Elementals { get; }
    public static MapsData Maps { get; }

    static State()
    {
        List<Elemental> allElementals = DataService.Instance.LoadData<List<Elemental>>(FileName.Elementals, false);
        List<Map> allMaps = DataService.Instance.LoadData<List<Map>>(FileName.Maps, false);
        GameState gs = DataService.Instance.LoadData<GameState>(FileName.State, true);

        lastEncounterDate = gs.lastEncounterDate.Year == 1 ? DateTime.Now : gs.lastEncounterDate;
        level = gs.level == 0 ? 1 : gs.level;
        experience = gs.experience;
        essence = gs.essence;
        gold = gs.gold;
        orbs = gs.orbs;
        lastCaught = lastCaught;
        Elementals = new ElementalsData(allElementals, gs.elementalEnteries);
        Maps = new MapsData(allMaps, gs.mapsProgression, gs.currentMapId);
        party = gs.party ?? new Party();
    }

    public static bool IsMaxLevel()
    {
        return level == MAX_LEVEL;
    }

    public static int ExpToLevelUp(int level)
    {
        return (int)(Math.Round((Math.Pow(level, 3) + level * 200) / 100.0) * 100);
    }

    private static bool ShouldToLevelUp()
    {
        return experience >= ExpToLevelUp(level);
    }

    public static void GainExperience(int exp)
    {
        experience += exp;

        while (ShouldToLevelUp() && !IsMaxLevel())
        {
            experience -= ExpToLevelUp(level);
            level++;
            GameEvents.LevelUp();
        }
    }

    public static void UpdateEssence(int amount)
    {
        essence = (essence + amount >= 0) ? essence + amount : 0;
        GameEvents.EssenceUpdated();

    }

    public static void UpdateGold(int amount)
    {
        gold = (gold + amount >= 0) ? gold + amount : 0;
        GameEvents.GoldUpdated();
    }

    public static void UpdateOrbs(int amount)
    {
        orbs = (orbs + amount >= 0) ? orbs + amount : 0;
    }

    public static void UpdatelastEncounterDate(DateTime date)
    {
        lastEncounterDate = date;
    }

    public static void UpdateElementalCaught(ElementalId elementalId, bool isNaturalEncounter = true)
    {
        lastCaught = elementalId;
        GameEvents.ElementalCaught();

        if (isNaturalEncounter)
        {
            lastEncounterDate = DateTime.Now;
            GameEvents.TriggerElementalToast();
        }
    }

    public static void Save()
    {
        GameState gs = new GameState()
        {
            lastEncounterDate = lastEncounterDate,
            currentMapId = Maps.currentMapId,
            level = level,
            experience = experience,
            essence = essence,
            gold = gold,
            orbs = orbs,
            elementalEnteries = Elementals.entries,
            mapsProgression = Maps.progressions,
            party = party,
            lastCaught = lastCaught,
        };

        DataService.Instance.SaveData(FileName.State, true, gs);
    }
}