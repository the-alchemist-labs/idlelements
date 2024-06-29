using System;

[Serializable]
public class Party
{
    public ElementalId? First { get; private set; }
    public ElementalId? Second { get; private set; }
    public ElementalId? Third { get; private set; }

    public int Length { get { return 3; } }

    public Party(ElementalId? first = null, ElementalId? second = null, ElementalId? third = null)
    {
        First = first;
        Second = second;
        Third = third;
    }

    public ElementalId? GetPartyMember(int slot)
    {
        switch (slot)
        {
            case 0:
                return First;
            case 1:
                return Second;
            case 2:
                return Third;
            default:
                return null;
        }
    }

    public void SetPartyMember(int slot, ElementalId? id)
    {
        _setPartyMember(slot, id);
        GameEvents.PartyUpdated();
    }

    public bool IsInParty(ElementalId? id)
    {
        return First == id || Second == id || Third == id;
    }

    private void _setPartyMember(int slot, ElementalId? id)
    {
        switch (slot)
        {
            case 0:
                First = id;
                return;

            case 1:
                Second = id;
                return;

            case 2:
                Third = id;
                return;
        }
    }
}