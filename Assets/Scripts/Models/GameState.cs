using System;
using System.Collections.Generic;

[Serializable]
public struct GameState
{
    public DateTime lastEncounterDate;
    public MapId currentMapId;
    public int level;
    public int experience;
    public int essence;
    public int gold;
    public int orbs;
    public Party party;
    public ElementalId lastCaught;
    public List<ElementalEntry> elementalEnteries;
    public List<MapProgression> mapsProgression;
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
