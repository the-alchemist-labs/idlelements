using System;

[Serializable]
public class DeckRegistry
{
    public ElementalId id { get; private set; }
    public bool isRegistered { get; private set; }
    public int tokens { get; private set; }

    public DeckRegistry(ElementalId id, bool isRegistered = false, int tokens = 0)
    {
        this.id = id;
        this.isRegistered = isRegistered;
        this.tokens = tokens;
    }

    public void Register()
    {
        isRegistered = true;
    }

    public void UpdateTokensBy(int updateBy)
    {
        tokens = (tokens + updateBy >= 0) ? tokens + updateBy : 0;
    }
}