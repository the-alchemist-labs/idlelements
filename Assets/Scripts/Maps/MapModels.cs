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
        return ElementalCatalog.Instance.GetElemental(encounterPool[randomIndex]);
    }
}


[Serializable]
public class MapProgression
{
    public MapId id;
    public int templeLevel = 1;
    public int goldMineLevel = 1;
    public int essenceLabLevel = 1;
    public bool isUnlocked = false;

    public void TempleLevelUp()
    {
        templeLevel++;
    }

    public void GoldMineLevelUp()
    {
        goldMineLevel++;
    }

    public void EssenceLabLevelUp()
    {
        essenceLabLevel++;
    }

    public void UnlockMap()
    {
        isUnlocked = true;
    }
}

[Serializable]
public class MapManagerState
{
    public List<MapProgression> progressions { get; set; }
    public MapId currentMapId { get; set; }


   public MapManagerState()
    {
        progressions = new List<MapProgression>();
        currentMapId = MapId.FireWater;
    }

   public MapManagerState(List<MapProgression> progressions, MapId currentMapId)
    {
        this.progressions = progressions;
        this.currentMapId = currentMapId;
    }
}
