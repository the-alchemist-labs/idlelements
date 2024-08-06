using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AfkGainsPanel : MonoBehaviour
{
    [SerializeField] GameObject itemsSection;
    [SerializeField] Transform itemsContainer;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] TMP_Text idleTimeText;
    [SerializeField] TMP_Text essenceText;
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text expText;

    private IdleRewards _rewards;

    void OnDestroy()
    {
        AfkGains.AcceptRewards(_rewards);
    }

    public void DisplayAfkGains()
    {
        gameObject.SetActive(true);
        int timesCleared = AfkGains.GetTimesCleared();
        _rewards = AfkGains.CalculateRewards(timesCleared);
        IdleBattleManager.Instance.UpdateLastRewardTimestam(DateTime.Now);
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        gameObject.SetActive(true);
        PopulateItems();
        UpdateText();
    }

    private void PopulateItems()
    {
        if (!_rewards.Elementokens.Any() && !_rewards.Balls.Any())
        {
            itemsSection.SetActive(false);
            return;
        }

        itemsContainer.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (KeyValuePair<ElementType, int> reward in _rewards.Elementokens)
        {
            GameObject tokens = Instantiate(itemPrefab, itemsContainer);
            if (tokens.TryGetComponent(out ItemRewardPrefab item))
            {
                item.SetItemPrefab(reward.Key, reward.Value);
            }
        }

        foreach (KeyValuePair<BallId, int> reward in _rewards.Balls)
        {
            GameObject balls = Instantiate(itemPrefab, itemsContainer);
            if (balls.TryGetComponent(out ItemRewardPrefab item))
            {
                item.SetItemPrefab(reward.Key, reward.Value);
            }
        }
    }

    private void UpdateText()
    {
        idleTimeText.text = $"You idle rewards for {_rewards.IdleTime}";
        goldText.text = TextUtil.NumberFormatter(_rewards.Gold);
        essenceText.text = TextUtil.NumberFormatter(_rewards.Essence);
        expText.text = TextUtil.NumberFormatter(_rewards.Experience);
    }

    public void AcceptRewards()
    {
        AfkGains.AcceptRewards(_rewards);
        gameObject.SetActive(false);
    }
}
