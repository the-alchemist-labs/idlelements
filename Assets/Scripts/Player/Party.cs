using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

[Serializable]
public class Party
{
    public ElementalId First { get; private set; }
    public ElementalId Second { get; private set; }
    public ElementalId Third { get; private set; }

    public int MaxSize { get { return 3; } }

    public Party(ElementalId First = ElementalId.None, ElementalId Second = ElementalId.None, ElementalId Third = ElementalId.None)
    {
        this.First = First;
        this.Second = Second;
        this.Third = Third;
    }

    public ElementalId GetPartyMember(int slot)
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
                return ElementalId.None;
        }
    }

    public void SetPartyMember(int slot, ElementalId id)
    {
        _setPartyMember(slot, id);
        GameEvents.PartyUpdated();
    }

    public ElementalId[] GetEligiblePartyMembers()
    {
        return ElementalManager.Instance.entries
            .Where(entry => entry.isCaught)
            .Select(entry => entry.id)
            .OrderBy(id => Player.Instance.Party.IsInParty(id))
            .ToArray();
    }

    public float GetPartyBonusMultipier(BonusResource resource)
    {
        return new List<ElementalId?> { First, Second, Third }
        .Where(e => e != ElementalId.None)
        .Select(id => ElementalCatalog.Instance.GetElemental(id.Value))
        .Where(e => e.idleBonus != null)
        .Where(e => e.idleBonus?.resource == resource)
        .Sum(e => e.idleBonus.amount);
    }

    public bool IsInParty(ElementalId? id)
    {
        return First == id || Second == id || Third == id;
    }

    public int PartyPower()
    {
        IEnumerable<ElementalId> party = new[] { First, Second, Third }.Where(e => e != ElementalId.None);

        return party.Sum(m => GetMemberDPS(m, Player.Instance.Level));
    }

    private int GetMemberDPS(ElementalId id, int level)
    {
        Elemental elemental = ElementalCatalog.Instance.GetElemental(id);
        List<SkillId> skillIds = ElementalManager.Instance.GetSkills(id);
        List<ElementalSkill> skills = skillIds.Select(s => ElementalCatalog.Instance.GetSkill(s)).ToList();
        int maxDamagSkill = skills.Max(s => s.ImpactValue);
        return (elemental.Stats.Attack + level) * maxDamagSkill;
    }

    private void _setPartyMember(int slot, ElementalId id)
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