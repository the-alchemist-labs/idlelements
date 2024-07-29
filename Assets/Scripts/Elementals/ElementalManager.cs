using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElementalManager : MonoBehaviour
{
    public static ElementalManager Instance { get; private set; }
    public List<ElementalEntry> entries { get; private set; }
    public Encounter lastEncounter { get; private set; }
    public Dictionary<ElementalId, List<SkillId>> equipedSkills { get; private set; }
    public int elementalCaught { get { return entries.Count(entry => entry.isCaught); } }

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
        ElementalManagerState state = DataService.Instance.LoadData<ElementalManagerState>(FileName.ElementalManagerState, true);

        entries = state.entries;
        lastEncounter = state.lastEncounter;
        equipedSkills = state.equipedSkills;
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
        Elemental elemental = ElementalCatalog.Instance.GetElemental(id);

        return elemental.evolution != null
        && entry.tokens >= elemental.evolution.tokensCost
        && Player.Instance.Resources.Essence >= elemental.evolution.essenceCost;
    }

    public void Evolve(ElementalId id)
    {
        Elemental elemental = ElementalCatalog.Instance.GetElemental(id);

        UpdateElementalTokens(id, -elemental.evolution.tokensCost);
        Player.Instance.Resources.UpdateEssence(-elemental.evolution.essenceCost);
        UpdateElementalTokens(elemental.evolution.evolveTo, 1);

        GameEvents.EssenceUpdated();
        GameEvents.TokensUpdated();
    }

    public void UpdatelastEncounter(Encounter encounter)
    {
        lastEncounter = encounter;
    }

    public bool CatchElemental(ElementalId elementalId, BallId ballId)
    {
        Elemental elemental = ElementalCatalog.Instance.GetElemental(elementalId);
        Ball ball = InventoryCatalog.Instance.GetBall(ballId);

        float bonusCatchRate = 0.1f;
        float totalCatchRate = elemental.catchRate * ball.CatchRate + bonusCatchRate;
        float randomValue = UnityEngine.Random.Range(0f, 1f);
        bool isCaught = totalCatchRate >= randomValue;

        if (isCaught) GameEvents.ElementalCaught();

        return isCaught;
    }

    public List<SkillId> GetSkills(ElementalId elementalId)
    {
        return equipedSkills.TryGetValue(elementalId, out List<SkillId> value) ? value : new List<SkillId> { SkillId.Default };
    }

    public List<SkillId> GetSkills(MinimentalId minimentalId)
    {
        return ElementalCatalog.Instance.GetElemental(minimentalId).Skills;
    }

    public void EquipSkill(ElementalId elementalId, SkillId skillIdToAdd, SkillId skillIdToRemove)
    {
        if (!equipedSkills.ContainsKey(elementalId))
        {
            equipedSkills.Add(elementalId, new List<SkillId>());
        }

        if (equipedSkills.TryGetValue(elementalId, out List<SkillId> skillsList))
        {
            skillsList.Remove(skillIdToRemove);
            skillsList.Add(skillIdToAdd);
        }
    }
}
