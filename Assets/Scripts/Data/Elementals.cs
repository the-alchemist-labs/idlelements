using System.Collections.Generic;
using Unity.VisualScripting;

public class ElementalsData
{
    public List<Elemental> all { get; private set; }
    public List<ElementalEntry> entries { get; private set; }

    public ElementalsData(List<Elemental> elementals, List<ElementalEntry> elementalEntries)
    {
        all = elementals;
        entries = elementalEntries ?? new List<ElementalEntry>();
    }

    public Elemental GetElement(ElementalId id)
    {
        return all.Find(el => el.id == id);
    }

    public bool IsElementalRegistered(ElementalId id)
    {
        return entries.Find(e => e.id == id)?.isCaught ?? false;
    }

    public void MarkElementalAsCaught(ElementalId id)
    {
        GetElementalEntry(id).isCaught = true;
    }

    public void UpdateElementalTokens(ElementalId id, int updateBy)
    {
        int tokens = GetElementalEntry(id).tokens;
        GetElementalEntry(id).tokens = (tokens + updateBy >= 0) ? tokens + updateBy : 0;
    }

    public ElementalEntry GetElementalEntry(ElementalId id)
    {
        ElementalEntry elemental = entries.Find(e => e.id == id);
        if (elemental == null)
        {
            elemental = new ElementalEntry() { id = id };
            entries.Add(elemental);
        }

        return elemental;
    }

    public bool CanEvolve(ElementalId id)
    {
        ElementalEntry entry = GetElementalEntry(id);
        Elemental elemental = GetElement(id);

        return elemental.evolution != null
        && entry.tokens >= elemental.evolution.tokensCost 
        && State.essence >= elemental.evolution.essenceCost;
    }

    public void Evolve(ElementalId id)
    {
        Elemental elemental = GetElement(id);

        if (!CanEvolve(id))
        {
            return;
        }

        UpdateElementalTokens(id, -elemental.evolution.tokensCost);
        State.UpdateEssence(-elemental.evolution.essenceCost);
        UpdateElementalTokens(elemental.evolution.evolveTo, 1);

        GameEvents.EssenceUpdated();
        GameEvents.TokensUpdated();

        if (!IsElementalRegistered(elemental.evolution.evolveTo))
        {
            MarkElementalAsCaught(elemental.evolution.evolveTo);
            GameEvents.ElementalCaught();
        }
    }
}