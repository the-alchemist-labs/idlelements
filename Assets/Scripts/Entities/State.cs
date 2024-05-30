using System;
using System.Collections.Generic;
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

    public readonly static Dictionary<int, int> requiredExpToLevelUp = new Dictionary<int, int>()
    {
        { 1, 250 },
        { 2, 600 },
        { 3, 1000 },
        { 4, 2000 },
        { 5, 3500 },
        { 6, 10000000 },
    };

    static State()
    {
        GameState gs = DataService.Instance.LoadData<GameState>(FileName.State);
        lastEncounter = gs.lastEncounter;
        currentMap = gs.currentMap;
        level = gs.level;
        experience = gs.experience;
        essence = gs.essence;
        orbs = gs.orbs;
        inventory = gs.inventory;
    }

    public static void GainExperience(int exp)
    {
        if (level >= requiredExpToLevelUp.Count)
        {
            Debug.Log("Max level reached");
            return;
        }

        experience += exp;
        if (experience >= requiredExpToLevelUp[level])
        {
            if (level + 1 == requiredExpToLevelUp.Count)
            {
                experience = 0;

            }
            else
            {
                experience = experience - requiredExpToLevelUp[level];
                GainExperience(0);
            }
            level++;
            // trigger levelup behavior
        }
        Save();
    }

    public static void UpdateEssence(int amount)
    {
        essence = (essence + amount >= 0) ? essence + amount : 0;
        Save();
    }

    public static void UpdateOrbs(int amount)
    {
        orbs = (orbs + amount >= 0) ? orbs + amount : 0;
        Save();
    }

    public static void UpdateCurrentMap(MapId id)
    {
        currentMap = id;
        Save();
    }

    public static void UpdateLastEncounter(DateTime date)
    {
        lastEncounter = date;
        Save();
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
            inventory = inventory
        };

        DataService.Instance.SaveData(FileName.State, gs);
    }
}