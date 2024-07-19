using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElementalsData : MonoBehaviour
{
    public static ElementalsData Instance { get; private set; }
    public List<Elemental> all { get; private set; }
    public List<ElementalEntry> entries { get; private set; }
    public ElementalId lastCaught { get; private set; }
    public int elementalCaught { get { return entries.Count(entry => entry.isCaught); } }
    public DateTime lastEncounterDate { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        GameState gs = DataService.Instance.LoadData<GameState>(FileName.State, true);

        all = DataService.Instance.LoadData<List<Elemental>>(FileName.Elementals, false);
        entries = gs.elementalEnteries ?? new List<ElementalEntry>();
        lastCaught = gs.lastCaught;
        lastEncounterDate = gs.lastEncounterDate.Year == 1 ? DateTime.Now : gs.lastEncounterDate;
    }

    public Elemental GetElemental(ElementalId id)
    {
        return all.Find(el => el.id == id);
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

    public bool CanEvolve(ElementalId id)
    {
        ElementalEntry entry = GetElementalEntry(id);
        Elemental elemental = GetElemental(id);

        return elemental.evolution != null
        && entry.tokens >= elemental.evolution.tokensCost
        && ResourcesData.Instance.Essence >= elemental.evolution.essenceCost;
    }

    public void Evolve(ElementalId id)
    {
        Elemental elemental = GetElemental(id);

        Temple.ElementalCaught(elemental.evolution.evolveTo, false);
        UpdateElementalTokens(id, -elemental.evolution.tokensCost);
        ResourcesData.Instance.UpdateEssence(-elemental.evolution.essenceCost);
        UpdateElementalTokens(elemental.evolution.evolveTo, 1);

        GameEvents.EssenceUpdated();
        GameEvents.TokensUpdated();
    }

    public void UpdateElementalCaught(ElementalId elementalId, bool isNaturalEncounter = true)
    {
        lastCaught = elementalId;
        GameEvents.ElementalCaught();

        if (isNaturalEncounter)
        {
            UpdatelastEncounterDate(DateTime.Now);
            GameEvents.TriggerElementalToast();
        }
    }

    public void UpdatelastEncounterDate(DateTime date)
    {
        lastEncounterDate = date;
    }
}