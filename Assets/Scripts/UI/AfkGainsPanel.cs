using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AfkGainsPanel : MonoBehaviour
{
    public GameObject afkGainsPanel;
    public GameObject newElelemtalPrefab;
    public Transform newElelemtalContainer;
    public GameObject elementalTokensPrefab;
    public Transform elementalTokensContainer;
    public TMP_Text essenceText;
    public TMP_Text expText;

    private IdleRewards idleRewards;
    public void DisaplyAfkGains(IdleRewards rewards)
    {
        idleRewards = rewards;
        afkGainsPanel.SetActive(true);
        InstantiateNewCaught(rewards.newCatches);
        InstantiateElementalTokens(rewards.elementalTokens);
        essenceText.text = TextUtil.NumberFormatter(rewards.essence);
        expText.text = TextUtil.NumberFormatter(rewards.experience);
    }

    public void AcceptRewards()
    {
        GameActions.EarnRewardOfEncounters(idleRewards);
        afkGainsPanel.SetActive(false);
    }

    private void InstantiateNewCaught(List<ElementalId> newElementalIds)
    {
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
