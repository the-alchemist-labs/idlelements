using System;

[Serializable]
public class Elemental
{
    public ElementalId id { get; }
    public string name { get; }
    public ElementType type { get; }
    public float catchRate { get; }

    public int expGain { get; }
    public int essenceGain { get; }

    public Elemental(ElementalId id, string name, ElementType type, float catchRate, int expGain, int essenceGain)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.catchRate = catchRate;
        this.expGain = expGain;
        this.essenceGain = essenceGain;
    }

    public bool IsCaught(float catchModifier = 1.0f)
    {
        int random = UnityEngine.Random.Range(0, 100);
        return (catchModifier * random) >= catchRate;
    }
}
