using System;
using System.Collections.Generic;

public class State
{
    public static DateTime lastEncounter { get; set; }
    public static MapId currentMap { get; set; }
    public static int level { get; private set; }
    public static int experience { get; private set; }
    public static int essence { get; set; }
    public static int orbs { get; set; }
    public static List<Item> inventory { get; }

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
        experience += exp;
        // check for level up
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