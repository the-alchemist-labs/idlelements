using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public struct ElementalEncounter
{
    public ElementalId elementalId { get; }
    public float encounterChance { get; }

    public ElementalEncounter(ElementalId elementalId, float encounterChance)
    {
        this.elementalId = elementalId;
        this.encounterChance = encounterChance;
    }
}

[Serializable]
public class Map
{
    public MapId id { get; }
    public string name { get; }
    public ElementalEncounter[] elementalEncounters { get; }
    public int requiredLevel { get; }
    public ElementType[] mapElementalTypes { get; }
    public int catchesToComplete { get; }

    public Map(MapId id, string name, int requiredLevel, ElementalEncounter[] elementalEncounters, int catchesToComplete)
    {
        this.id = id;
        this.name = name;
        this.requiredLevel = requiredLevel;
        this.elementalEncounters = elementalEncounters;
        mapElementalTypes = new ElementType[]{ ElementType.Fire, ElementType.Water };
        this.catchesToComplete = catchesToComplete;
    }

    public Elemental GetEncounter()
    {
        List<ElementalId> encounterPool = elementalEncounters
            .SelectMany(e => Enumerable.Repeat(e.elementalId, (int)(e.encounterChance * 100)))
            .ToList();
        int randomIndex = UnityEngine.Random.Range(0, encounterPool.Count);
        return State.Elementals.GetElement(encounterPool[randomIndex]);
    }
}
