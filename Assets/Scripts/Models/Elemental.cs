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

    public bool isSeen { get; private set; } = false;
    public bool isRegistered { get; private set; } = false;
    public int tokens { get; private set; } = 0;

    public Elemental(ElementalId id, string name, ElementType type, float catchRate, int expGain, int essenceGain)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.catchRate = catchRate;
        this.expGain = expGain;
        this.essenceGain = essenceGain;
    }

    public void MarkAsSeen()
    {
        isSeen = true;
    }
    
    public void UpdateTokensBy(int updateBy)
    {
        tokens = (tokens + updateBy >= 0) ? tokens + updateBy : 0;
    }

    public bool Catch(float catchModifier = 1.0f)
    {   
        int random = UnityEngine.Random.Range(0, 100);
        bool isCaught = (catchModifier * random) >= catchRate;

        if (isCaught)
        {
            if (!isRegistered)
            {
                UpdateTokensBy(1);
            }
            else
            {
                isRegistered = true;
            }
        }
        return isCaught;
    }
}
