using System;

[Serializable]
public class DeckRegistry
{
    public ElementalId id { get; }
    public bool isRegistered { get; set; } = false;
    public int tokens { get; set; } = 0;

    public DeckRegistry(ElementalId id, bool isRegistered, int tokens) {
        this.id = id;
        this.isRegistered = isRegistered;
        this.tokens = tokens;
    }
}