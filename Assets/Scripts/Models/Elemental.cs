using System;

[Serializable]
public class Elemental
{
    public ElementalId id { get; }
    public string name { get; }
    public ElementType type { get; }
    public float catchRate { get; }

    public int expGain { get; }
    public int orbsGain { get; }

    public Elemental(ElementalId id, string name, ElementType type, float catchRate, int expGain, int orbsGain)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.catchRate = catchRate;
        this.expGain = expGain;
        this.orbsGain = orbsGain;
    }

    public bool Catch(float catchModifier = 1.0f)
    {
        int random = UnityEngine.Random.Range(0, 100);
        bool isCaught = (catchModifier * random) >= catchRate;

        return isCaught;
    }
}
