#nullable enable
using System;
using System.Collections.Generic;
using Newtonsoft.Json;


[Serializable]
public class ElementalEntry
{
    public ElementalId id;
    public bool isCaught = false;
    public int tokens = 0;
}

[Serializable]
public class ElementalManagerState
{
    public List<ElementalEntry> entries { get; private set; }
    public Encounter lastEncounter { get; private set; }
    public Dictionary<ElementalId, List<SkillId>> equipedSkills { get; private set; }

    public ElementalManagerState()
    {
        entries = new List<ElementalEntry>();
        lastEncounter = new Encounter();
        equipedSkills = new Dictionary<ElementalId, List<SkillId>>();
    }

    [JsonConstructor]
    public ElementalManagerState(List<ElementalEntry> entries, Encounter lastEncounter, Dictionary<ElementalId, List<SkillId>> equipedSkills)
    {
        this.entries = entries;
        this.lastEncounter = lastEncounter;
        this.equipedSkills = equipedSkills;
    }
}