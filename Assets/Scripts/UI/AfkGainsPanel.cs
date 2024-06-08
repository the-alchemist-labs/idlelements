using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AfkGainsPanel : MonoBehaviour
{
    public GameObject afkGainsPanel;
    public TMP_Text idleTimeText;
    public GameObject newElelemtalPrefab;
    public GameObject newCatchesContainer;
    public GameObject tokensContainer;

    public Transform newElelemtalContainer;
    public GameObject elementalTokensPrefab;
    public Transform elementalTokensContainer;
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
        if (newElementalIds.Count == 0) newCatchesContainer.SetActive(false);

        foreach (ElementalId id in newElementalIds)
        {
            GameObject newCaught = Instantiate(newElelemtalPrefab, newElelemtalContainer);
            if (newCaught.TryGetComponent(out ChangeImageToElemental item))
            {
                item.UpdateImageToElemental(id);
            }
        }
    }

    private void InstantiateElementalTokens(Dictionary<ElementalId, int> gainedTokensDictionary)
    {
        if (gainedTokensDictionary.Count == 0) tokensContainer.SetActive(false);

        foreach (KeyValuePair<ElementalId, int> kvp in gainedTokensDictionary)
        {
            GameObject tokens = Instantiate(elementalTokensPrefab, elementalTokensContainer);
            if (tokens.TryGetComponent(out ChangeImageToElemental image))
            {
                image.UpdateImageToElemental(kvp.Key);
            }
            if (tokens.TryGetComponent(out ChangeTokenLabel text))
            {
                text.UpdateTokenLabel(kvp.Value);
            }
        }
    }
}
