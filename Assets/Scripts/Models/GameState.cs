using System;
using System.Collections.Generic;

[Serializable]
public struct GameState
{
    public DateTime lastEncounter;
    public MapId currentMapId;
    public int level;
    public int experience;
    public int essence;
    public int gold;
    public int orbs;
    public List<ElementalEntry> elementalEnteries;
    public List<MapProgression> mapsProgression;
    public List<Item> inventory;
}

[Serializable]
public class ElementalEntry
{
    public ElementalId id;
    public bool isCaught = false;
    public int tokens = 0; 
}

[Serializable]
public class MapProgression
{
    public MapId id;
    public bool isCompleted  = false;
    public int templeLevel = 0;
    public int goldMineLevel = 0;
}

[Serializable]
public struct Item
{
    public int name;
    public int amount;
}
