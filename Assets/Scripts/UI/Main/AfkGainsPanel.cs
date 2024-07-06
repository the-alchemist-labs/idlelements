using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class IdleRewards
{
    public int totalCatches = 0;
    public int orbs = 0;
    public int gold = 0;
    public int essence = 0;
    public int experience = 0;
    public List<ElementalId> newCatches = new List<ElementalId>();
    public Dictionary<ElementalId, int> elementalTokens = new Dictionary<ElementalId, int>();
    public ElementalId lastCaught;
    public string IdleTime;
}

public class AfkGainsPanel : MonoBehaviour
{
    public GameObject afkGainsPanel;
    public GameObject newCatchesContainer;
    public GameObject tokensContainer;
    public GameObject elementalPrefab;

    public TMP_Text idleTimeText;
    public TMP_Text essenceText;
    public TMP_Text goldText;
    public TMP_Text expText;

    private bool isRewardAccepted = false;

    private IdleRewards rewards;


    void OnDestroy()
    {
        if (!isRewardAccepted) EarnRewardOfEncounters(rewards);
    }

    public void DisplayAfkGains()
    {
        int delta = GetEncounterDeltaTime();
        rewards = GetIdleRewards();
        State.UpdatelastEncounterDate(DateTime.Now.AddSeconds(-delta));

        UpdatePanel();
    }

    public void UpdatePanel()
    {
        afkGainsPanel.SetActive(true);

        UpdateNewCaughtContainer(rewards.newCatches);
        UpdateElementalTokensContainer(rewards.elementalTokens);

        idleTimeText.text = $"You idle rewards for {rewards.IdleTime}";
        goldText.text = TextUtil.NumberFormatter(rewards.gold);
        essenceText.text = TextUtil.NumberFormatter(rewards.essence);
        expText.text = TextUtil.NumberFormatter(rewards.experience);
    }

    public void AcceptRewards()
    {
        EarnRewardOfEncounters(rewards);
        isRewardAccepted = true;
        afkGainsPanel.SetActive(false);
    }

    private void UpdateNewCaughtContainer(List<ElementalId> newElementalIds)
    {
        if (newElementalIds.Count == 0)
        {
            newCatchesContainer.SetActive(false);
            return;
        }

        Transform containerTransform = newCatchesContainer.transform.Find("NewElementalsContainer");
        containerTransform.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (ElementalId id in newElementalIds)
        {
            GameObject newCaught = Instantiate(elementalPrefab, containerTransform);
            if (newCaught.TryGetComponent(out ElementaAfkRewardsPrefub item))
            {
                item.UpdatePrefub(id);
            }
        }
    }

    private void UpdateElementalTokensContainer(Dictionary<ElementalId, int> gainedTokensDictionary)
    {
        if (gainedTokensDictionary.Count == 0)
        {
            tokensContainer.SetActive(false);
            return;
        }

        Transform containerTransform = tokensContainer.transform.Find("TokensContainer");
        containerTransform.transform.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (KeyValuePair<ElementalId, int> kvp in gainedTokensDictionary)
        {
            GameObject tokens = Instantiate(elementalPrefab, containerTransform);
            if (tokens.TryGetComponent(out ElementaAfkRewardsPrefub item))
            {
                item.UpdatePrefub(kvp.Key, kvp.Value);
            }
        }
    }

    private void EarnRewardOfEncounters(IdleRewards rewards)
    {
        rewards.newCatches.ForEach(c => State.Elementals.MarkElementalAsCaught(c));
        rewards.elementalTokens.ToList().ForEach(c => State.Elementals.UpdateElementalTokens(c.Key, c.Value));
        State.GainExperience(rewards.experience);
        State.UpdateEssence(rewards.essence);
        State.UpdateGold(rewards.gold);

        if (rewards.lastCaught != 0)
        {
            Temple.ElementalCaught(rewards.lastCaught, false);
        }
    }

    private IdleRewards GetIdleRewards()
    {
        float elapsedSeconds = Temple.GetSecondsSincelastEncounterDate(State.lastEncounterDate);

        System.Random random = new System.Random();
        IdleRewards rewards = new IdleRewards();
        int encounters = GetNumberOfEncounters(elapsedSeconds);

        ElementalEncounter[] elementalEncounters = State.Maps.currentMap.elementalEncounters
            .OrderByDescending(e => e.encounterChance)
            .ToArray();

        ElementalId[] encounterIds = Enumerable
        .Range(0, encounters)
        .Select(_ => Temple.GetEncounter(elementalEncounters))
        .ToArray();

        foreach (ElementalId elementalId in encounterIds)
        {
            if (!State.Elementals.IsElementalRegistered(elementalId) && !rewards.newCatches.Contains(elementalId))
            {
                rewards.newCatches.Add(elementalId);
            }

            rewards.elementalTokens[elementalId] = rewards.elementalTokens.GetValueOrDefault(elementalId, 0) + 1;
            rewards.experience += 0;

        }

        rewards.essence = EssenceLab.GetTotalEssenceFromAllMaps() * (int)(elapsedSeconds / EssenceLab.incomeLoopSeconds);
        rewards.gold = GoldMine.GetTotalGoldGains() * (int)(elapsedSeconds / GoldMine.incomeLoopSeconds);
        rewards.experience = GetTotalExp(encounterIds);
        rewards.lastCaught = encounterIds.Any() ? encounterIds.Last() : 0;
        rewards.IdleTime = TextUtil.FormatSecondsToTimeString(elapsedSeconds);
        return rewards;
    }

    private int GetTotalExp(ElementalId[] elementalIds)
    {
        Dictionary<ElementalId, int> expCache = new Dictionary<ElementalId, int>();
        int totalExp = 0;

        foreach (ElementalId elementalId in elementalIds)
        {
            if (!expCache.TryGetValue(elementalId, out int exp))
            {
                exp = State.Elementals.GetElemental(elementalId).expGain;
                expCache[elementalId] = exp;
            }
            totalExp += exp;
        }

        return totalExp;
    }

    private int GetEncounterDeltaTime()
    {
        float elapedsSeconds = Temple.GetSecondsSincelastEncounterDate(State.lastEncounterDate);
        int encounters = GetNumberOfEncounters(elapedsSeconds);
        int encountersTime = encounters * Temple.GetEncounterSpeed();
        return (int)elapedsSeconds - encountersTime;
    }

    private int GetNumberOfEncounters(float elapedsSeconds)
    {
        return (int)elapedsSeconds / Temple.GetEncounterSpeed();
    }
}