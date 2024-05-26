using System;

[Serializable]
public class Elemental
{
    public int idNum { get; }
    public string name { get; }
    public ElementType type { get; }
    public float catchRate { get; }

    public int expGain { get; }
    public int goldGain { get; }

    public bool isRegistered { get; set; } = false;
    public int tokens { get; set; } = 0;

    public Elemental(int idNum, string name, ElementType type, float catchRate, int expGain, int goldGain)
    {
        this.idNum = idNum;
        this.name = name;
        this.type = type;
        this.catchRate = catchRate;
        this.expGain = expGain;
        this.goldGain = goldGain;
    }

    public bool IsCaught(float catchModifier = 1.0f)
    {
        int random = UnityEngine.Random.Range(0, 100);
        return (catchModifier * random) >= catchRate;
    }
}
