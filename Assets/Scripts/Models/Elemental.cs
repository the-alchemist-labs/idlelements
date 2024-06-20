using System;

[Serializable]
public class Elemental
{
    public ElementalId id { get; }
    public string name { get; }
    public ElementType type { get; }

    public int expGain { get; }
    public int orbsGain { get; }

    public Elemental(ElementalId id, string name, ElementType type, int expGain, int orbsGain)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.expGain = expGain;
        this.orbsGain = orbsGain;
    }
}
