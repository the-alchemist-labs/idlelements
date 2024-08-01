using System;
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

    private IdleRewards rewards;

    void OnDestroy()
    {
        AfkGains.AcceptRewards(rewards);
    }

    public void DisplayAfkGains()
    {
        gameObject.SetActive(true);
        int timesCleared = AfkGains.GetTimesCleared();
        rewards = AfkGains.CalculateRewards(timesCleared);
        IdleBattleManager.Instance.UpdateLastRewardTimestam(DateTime.Now);
    }

    public void UpdatePanel()
    {
        afkGainsPanel.SetActive(true);

        idleTimeText.text = $"You idle rewards for {rewards.IdleTime}";
        goldText.text = TextUtil.NumberFormatter(rewards.Gold);
        essenceText.text = TextUtil.NumberFormatter(rewards.Essence);
        expText.text = TextUtil.NumberFormatter(rewards.Experience);
    }

    public void AcceptRewards()
    {
        AfkGains.AcceptRewards(rewards);
        // play sound & animations
        afkGainsPanel.SetActive(false);
    }
}