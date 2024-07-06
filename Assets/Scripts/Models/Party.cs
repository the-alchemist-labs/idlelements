using System;
using System.Collections.Generic;
using System.Linq;

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

    public ElementalId[] GetEligiblePartyMembers()
    {
        return State.Elementals.entries
            .Where(entry => entry.isCaught)
            .Select(entry => entry.id)
            .OrderBy(id => State.party.IsInParty(id))
            .ToArray();
    }

    public float GetPartyBonusMultipier(BonusResource resource)
    {
        return new List<ElementalId?> { First, Second, Third }
        .Where(e => e != null)
        .Select(id => State.Elementals.GetElemental(id.Value))
        .Where(e => e.idleBonus != null)
        .Where(e => e.idleBonus?.resource == resource)
        .Sum(e => e.idleBonus.amount);
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