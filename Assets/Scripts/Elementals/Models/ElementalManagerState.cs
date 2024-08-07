using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class ElementalManagerState
{
    public List<ElementalEntry> entries { get; private set; }
    public Encounter lastEncounter { get; private set; }
    public Dictionary<ElementalId, SkillId?[]> equipedSkills { get; private set; }

    public ElementalManagerState()
    {
        entries = new List<ElementalEntry>();
        lastEncounter = new Encounter();
        equipedSkills = new Dictionary<ElementalId, SkillId?[]>();
    }

    [JsonConstructor]
    public ElementalManagerState(List<ElementalEntry> entries, Encounter lastEncounter, Dictionary<ElementalId, SkillId?[]> equipedSkills)
    {
        this.entries = entries;
        this.lastEncounter = lastEncounter ?? new Encounter();
        this.equipedSkills = equipedSkills ?? new Dictionary<ElementalId, SkillId?[]>();;
    }
}

[Serializable]
public class ElementalEntry
{
    public ElementalId id;
    public bool isCaught = false;
    public int tokens = 0;
}
