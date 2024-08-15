using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text taskText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Image rewardImage;
    [SerializeField] private TMP_Text rewardAmountText;
    [SerializeField] private GameObject claimed;

    private Daily _daily;
    private bool _isCompleted;
    public void Init(DailyProgress dailyProgress)
    {
        Daily daily = DailiesManager.Instance.GetDaily(dailyProgress.Id);
        _daily = daily;
        _isCompleted = dailyProgress.IsCompleted;
        int progress = dailyProgress.Progress > daily.RequiredToComplete
            ? daily.RequiredToComplete
            : dailyProgress.Progress;
        taskText.text = daily.Description;
        progressSlider.value = (float)dailyProgress.Progress /(float)daily.RequiredToComplete;
        progressText.text = $"{TextUtil.NumberFormatter(progress)}/{TextUtil.NumberFormatter( daily.RequiredToComplete)}";
        rewardImage.sprite = Resources.Load<Sprite>("Sprites/Inventory/Balls/MasterBall");
        rewardAmountText.text = $"X{TextUtil.NumberFormatter(daily.Reward.Amount)}";
        claimed.SetActive(dailyProgress.WasClaimed);
    }

    public void Claim()
    {
        if (_isCompleted)
        {
            RewardService.ClaimReward(_daily.Reward);
            DailiesManager.Instance.DailyClaimed(_daily.Id);
            GameEvents.DailyUpdated();
        }
    }
}
