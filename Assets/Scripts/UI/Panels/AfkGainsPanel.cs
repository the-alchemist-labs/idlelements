using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

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

    private IdleRewards idleRewards;
    public void DisaplyAfkGains(IdleRewards rewards, string idleTime)
    {
        idleRewards = rewards;
        afkGainsPanel.SetActive(true);
        InstantiateNewCaught(rewards.newCatches);
        InstantiateElementalTokens(rewards.elementalTokens);
        idleTimeText.text = $"You idle rewards for {idleTime}";
        goldText.text = TextUtil.NumberFormatter(rewards.gold);
        essenceText.text = TextUtil.NumberFormatter(rewards.essence);
        expText.text = TextUtil.NumberFormatter(rewards.experience);
    }

    public void AcceptRewards()
    {
        GameActions.EarnRewardOfEncounters(idleRewards);
        isRewardAccepted = true;
        afkGainsPanel.SetActive(false);
    }

    void OnDestroy()
    {
        if (!isRewardAccepted) GameActions.EarnRewardOfEncounters(idleRewards);
    }

    private void InstantiateNewCaught(List<ElementalId> newElementalIds)
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

    private void InstantiateElementalTokens(Dictionary<ElementalId, int> gainedTokensDictionary)
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
}
