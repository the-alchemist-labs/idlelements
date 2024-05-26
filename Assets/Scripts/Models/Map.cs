using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Map
{
    public string mapName;
    public ElementalEncounter[] elementalEncounters;
    public int requiredLevel;

    public Map(string mapName, int requiredLevel, ElementalEncounter[] elementalEncounters)
    {
        this.mapName = mapName;
        this.requiredLevel = requiredLevel;
        this.elementalEncounters = elementalEncounters;
    }

    public Elemental GetEncounter()
    {
        List<Elemental> encounterPool = elementalEncounters
            .SelectMany(e => Enumerable.Repeat(e.elemental, (int)(e.encounterChance * 100)))
            .ToList();
        int randomIndex = UnityEngine.Random.Range(0, encounterPool.Count);
        return encounterPool[randomIndex];
    }
}

[Serializable]
public struct ElementalEncounter
{
    public Elemental elemental;
    public float encounterChance;

    public ElementalEncounter(Elemental elemental, float encounterChance)
    {
        this.elemental = elemental;
        this.encounterChance = encounterChance;
    }
}