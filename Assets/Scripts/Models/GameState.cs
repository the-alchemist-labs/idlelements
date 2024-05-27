using System;
using System.Collections.Generic;

[Serializable]
public struct GameState
{
    public DateTime lastEncounter;
    public MapId currentMap;
    public int level;
    public int experience;
    public int essence;
    public int orbs;
    public List<Item> inventory;
}


[Serializable]
public class Item
{
    public int name;
    public int amount;
}
