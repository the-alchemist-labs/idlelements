#nullable enable
using System;
using System.Collections.Generic;

[Serializable]
public class Evolution
{
    public ElementalId evolveTo { get; }
    public int tokensCost { get; }
    public int essenceCost { get; }

    public Evolution(ElementalId evolveTo, int tokensCost, int essenceCost)
    {
        this.evolveTo = evolveTo;
        this.tokensCost = tokensCost;
        this.essenceCost = essenceCost;
    }
}

[Serializable]
public class IdleBonus
{
    public BonusResource resource { get; }
    public float amount { get; }

    public IdleBonus(BonusResource resource, float amount)
    {
        this.resource = resource;
        this.amount = amount;
    }
}

[Serializable]
public class Elemental
{
    public ElementalId id { get; }
    public string name { get; }
    public ElementType type { get; }
    public Evolution? evolution { get; }
    public int expGain { get; }
    public int orbsGain { get; }
    public IdleBonus? idleBonus { get; }


    public Elemental(ElementalId id, string name, Evolution evolution, ElementType type, int expGain, int orbsGain, IdleBonus idleBonus)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.expGain = expGain;
        this.orbsGain = orbsGain;
        this.evolution = evolution;
        this.idleBonus = idleBonus;
    }
}

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
    public ElementalId lastCaught { get; private set; }
    public DateTime lastEncounterDate { get; private set; }

    public ElementalManagerState()
    {
        entries = new List<ElementalEntry>();
        lastCaught = ElementalId.None;
        lastEncounterDate = DateTime.Now;
    }

    public ElementalManagerState(List<ElementalEntry> entries, ElementalId lastCaught, DateTime lastEncounterDate)
    {
        this.entries = entries;
        this.lastCaught = lastCaught;
        this.lastEncounterDate = lastEncounterDate;
    }
}
