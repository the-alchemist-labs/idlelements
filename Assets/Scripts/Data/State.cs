using System;
using System.Collections.Generic;

public static class State
{
    private static int baseEncounterSpeed = 300;

    public static DateTime lastEncounterDate { get; private set; }

    public static int level { get; private set; }
    public static int experience { get; private set; }
    public static int essence { get; private set; }
    public static int gold { get; private set; }
    public static int orbs { get; private set; }
    public static List<Item> inventory { get; }

    public static Elemental[] party { get; }
    public static ElementalsData Elementals { get; }
    public static MapsData Maps { get; }

    public readonly static Dictionary<int, int> requiredExpToLevelUp = new Dictionary<int, int>()
    {
        { 1, 250 },
        { 2, 600 },
        { 3, 1000 },
        { 4, 2000 },
        { 5, 3500 },
        { 6, 5000 },
        { 7, 8000 },
        { 8, 10000 },
        { 9, 16000 },
        { 10, 20000 },
    };

    static State()
    {
        List<Elemental> allElementals = DataService.Instance.LoadData<List<Elemental>>(FileName.Elementals);
        List<Map> allMaps = DataService.Instance.LoadData<List<Map>>(FileName.Maps);
        GameState gs = DataService.Instance.LoadData<GameState>(FileName.State);

        lastEncounterDate = gs.lastEncounterDate.Year == 1 ? DateTime.Now : gs.lastEncounterDate;
        level = gs.level == 0 ? 1 : gs.level;
        experience = gs.experience;
        essence = gs.essence;
        gold = gs.gold;
        orbs = gs.orbs;
        inventory = gs.inventory;
        party = party;
        Elementals = new ElementalsData(allElementals, gs.elementalEnteries);
        Maps = new MapsData(allMaps, gs.mapsProgression, gs.currentMapId);
    }

    public static int GetEncounterSpeed()
    {
        float encounterSpeedModifier = Temple.GetTotalBuff() / 100;
        return (int)(baseEncounterSpeed / (1 + encounterSpeedModifier));
    }

    public static bool IsMaxLevel()
    {
        return level == requiredExpToLevelUp.Count;
    }

    private static bool ShouldToLevelUp()
    {
        return experience >= requiredExpToLevelUp[level];
    }

    public static void GainExperience(int exp)
    {
        experience += exp;

        while (true)
        {
            if (ShouldToLevelUp() && !IsMaxLevel())
            {
                experience -= requiredExpToLevelUp[level];
                level++;
                // TODO: trigger levelup behavior
            }
            else
            {
                break;
            }
        }
    }

    public static void UpdateEssence(int amount)
    {
        essence = (essence + amount >= 0) ? essence + amount : 0;
    }

    public static void UpdateGold(int amount)
    {
        gold = (gold + amount >= 0) ? gold + amount : 0;
    }

    public static void UpdateOrbs(int amount)
    {
        orbs = (orbs + amount >= 0) ? orbs + amount : 0;
    }

    public static void UpdatelastEncounterDate(DateTime date)
    {
        lastEncounterDate = date;
    }

    public static int GetSecondsUntilNextEncounter()
    {

        int secondsSincelastEncounterDate = (int)(DateTime.Now - lastEncounterDate).TotalSeconds;
        return (GetEncounterSpeed() - secondsSincelastEncounterDate) % GetEncounterSpeed();
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
            inventory = inventory,
            elementalEnteries = Elementals.entries,
            mapsProgression = Maps.progressions,
            party = party,
        };

        DataService.Instance.SaveData(FileName.State, gs);
    }
}