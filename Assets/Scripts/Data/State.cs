using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;



public static class State
{
    public static DateTime lastEncounter { get; private set; }
    public static MapId currentMap { get; private set; }
    public static int level { get; private set; }
    public static int experience { get; private set; }
    public static int essence { get; private set; }
    public static int orbs { get; private set; }
    public static List<Item> inventory { get; }
    public static List<ElementalEntry> elementals { get; }
    public static List<MapProgression> maps { get; }

    public readonly static Dictionary<int, int> requiredExpToLevelUp = new Dictionary<int, int>()
    {
        { 1, 250 },
        { 2, 600 },
        { 3, 1000 },
        { 4, 2000 },
        { 5, 3500 },
        { 6, 5000 },
        { 7, 8000 },
        { 8, 12000 },
        { 9, 16000 },
        { 10, 20000 },
    };

    static State()
    {
        GameState gs = DataService.Instance.LoadData<GameState>(FileName.State);
        lastEncounter = gs.lastEncounter;
        currentMap = gs.currentMap;
        level = gs.level == 0 ? 1 : gs.level;
        experience = gs.experience;
        essence = gs.essence;
        orbs = gs.orbs;
        inventory = gs.inventory;
        elementals = gs.elementals;
        maps = gs.maps;
    }

    public static bool IsMaxLevel()
    {
        return level == requiredExpToLevelUp.Count;
    }

    public static void GainExperience(int exp)
    {
        if (level >= requiredExpToLevelUp.Count)
        {
            return;
        }

        experience += exp;
        if (experience >= requiredExpToLevelUp[level])
        {
            if (level + 1 == requiredExpToLevelUp.Count)
            {
                experience = requiredExpToLevelUp[requiredExpToLevelUp.Count];
            }
            else
            {
                experience = experience - requiredExpToLevelUp[level];
                GainExperience(0);
            }
            level++;
            // trigger levelup behavior
        }
    }

    public static void UpdateEssence(int amount)
    {
        essence = (essence + amount >= 0) ? essence + amount : 0;
    }

    public static void UpdateOrbs(int amount)
    {
        orbs = (orbs + amount >= 0) ? orbs + amount : 0;
    }

    public static void UpdateCurrentMap(MapId id)
    {
        currentMap = id;
    }

    public static void UpdateLastEncounter(DateTime date)
    {
        lastEncounter = date;
    }

    public static bool IsElementalRegistered(ElementalId id)
    {
        return elementals.Find(e => e.id == id)?.isSeen ?? false;
    }

    public static void MarkElementalAsSeen(ElementalId id)
    {
        GetElemental(id).isSeen = true;
    }

    public static void MarkElementalAsCaught(ElementalId id)
    {
        GetElemental(id).isCaught = true;
    }

    public static void UpdateElementalTokens(ElementalId id, int updateBy)
    {
        int tokens = GetElemental(id).tokens;
        GetElemental(id).tokens = (tokens + updateBy >= 0) ? tokens + updateBy : 0;
    }

    public static void UpdateMapProgression(int catches)
    {
        GetMapProgression(currentMap).catchProgression += catches;
    }

    public static MapProgression GetMapProgression(MapId id)
    {
        MapProgression mapProgression = maps.Find(m => m.id == id);
        if (mapProgression == null)
        {
            mapProgression = new MapProgression() { id = id };
            maps.Add(mapProgression);
        }

        return mapProgression;
    }

    public static ElementalEntry GetElemental(ElementalId id)
    {
        ElementalEntry elemental = elementals.Find(e => e.id == id);
        if (elemental == null)
        {
            elemental = new ElementalEntry() { id = id };
            elementals.Add(elemental);
        }

        return elemental;
    }

    public static void Save()
    {
        GameState gs = new GameState()
        {
            lastEncounter = lastEncounter,
            currentMap = currentMap,
            level = level,
            experience = experience,
            essence = essence,
            inventory = inventory,
            elementals = elementals,
            maps = maps
        };

        DataService.Instance.SaveData(FileName.State, gs);
    }
}