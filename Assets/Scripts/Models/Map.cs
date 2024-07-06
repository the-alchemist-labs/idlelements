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
    public BuildingSpecs goldMineSpecs { get; }
    public BuildingSpecs essenceLabSpecs { get; }
    public TempleSpecs templeSpecs { get; }

    public Map(
        MapId id,
        string name,
        int requiredLevel,
        ElementalEncounter[] elementalEncounters,
        int catchesToComplete,
        ElementType[] mapElementalTypes,
        BuildingSpecs goldMineSpecs,
        BuildingSpecs essenceLabSpecs,
        TempleSpecs templeSpecs
        )
    {
        this.id = id;
        this.name = name;
        this.requiredLevel = requiredLevel;
        this.elementalEncounters = elementalEncounters;
        this.mapElementalTypes = mapElementalTypes;
        this.catchesToComplete = catchesToComplete;
        this.goldMineSpecs = goldMineSpecs;
        this.essenceLabSpecs = essenceLabSpecs;
        this.templeSpecs = templeSpecs;
    }

    public Elemental GetEncounter()
    {
        List<ElementalId> encounterPool = elementalEncounters
            .SelectMany(e => Enumerable.Repeat(e.elementalId, (int)(e.encounterChance * 100)))
            .ToList();
        int randomIndex = UnityEngine.Random.Range(0, encounterPool.Count);
        return State.Elementals.GetElemental(encounterPool[randomIndex]);
    }
}
