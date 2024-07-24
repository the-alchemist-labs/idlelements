#nullable enable
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
    public float catchRate { get; }
    public Evolution? evolution { get; }
    public int expGain { get; }
    public int orbsGain { get; }
    public IdleBonus? idleBonus { get; }


    public Elemental(ElementalId id, string name, float catchRate, Evolution evolution, ElementType type, int expGain, int orbsGain, IdleBonus idleBonus)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.catchRate = catchRate;
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
public class Encounter
{
    const int MAX_CATCH_TRIES = 3;

    public ElementalId EncounterId { get; private set; }
    public int Tries { get; private set; }

    public Encounter()
    {
        EncounterId = ElementalId.None;
        Tries = 0;
    }

    [JsonConstructor]
    public Encounter(ElementalId EncounterId, int Tries)
    {
        this.EncounterId = EncounterId;
        this.Tries = Tries;
    }


    public void SetNewEncounter(ElementalId encounterId)
    {
        EncounterId = encounterId;
        Tries = 0;
    }

    public void UseTry()
    {
        Tries++;
    }

    public bool HasRemainingTries()
    {
        return Tries < MAX_CATCH_TRIES;
    }
}

[Serializable]
public class ElementalManagerState
{
    public List<ElementalEntry> entries { get; private set; }
    public Encounter lastEncounter { get; private set; }
    public ElementalManagerState()
    {
        entries = new List<ElementalEntry>();
        lastEncounter = new Encounter();
    }

    [JsonConstructor]
    public ElementalManagerState(List<ElementalEntry> entries, Encounter lastEncounter)
    {
        this.entries = entries;
        this.lastEncounter = lastEncounter;
    }
}
