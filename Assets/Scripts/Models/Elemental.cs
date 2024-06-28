using System;

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
public class Elemental
{
    public ElementalId id { get; }
    public string name { get; }
    public ElementType type { get; }
    public Evolution evolution { get; }
    public int expGain { get; }
    public int orbsGain { get; }

    public Elemental(ElementalId id, string name, Evolution evolution, ElementType type, int expGain, int orbsGain)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.expGain = expGain;
        this.orbsGain = orbsGain;
        this.evolution = evolution;
    }
}
