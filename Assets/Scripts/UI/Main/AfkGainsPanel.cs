using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AfkGainsPanel : BasePopup
{
    public override PopupId Id { get; } = PopupId.AfkGains;

    [SerializeField] GameObject itemsSection;
    [SerializeField] Transform itemsContainer;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] TMP_Text idleTimeText;
    [SerializeField] TMP_Text essenceText;
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text expText;

    private IdleRewards _idleRewards;
    private Dictionary<RewardId, int> _items;
    private int _gold;
    private int _essence;
    private int _exp;

    void Awake()
    {
        SetupCloseableBackground(true);
    }
    void OnDestroy()
    {
        AcceptRewards();
    }

    public void InitAfkGains()
    {
        int timesCleared = AfkGains.GetTimesCleared();
        _idleRewards = AfkGains.CalculateRewards(timesCleared);


        _gold = _idleRewards.Rewards.SingleOrDefault(r => r.Type == RewardType.Gold)?.Amount ?? 0;
        _essence = _idleRewards.Rewards.SingleOrDefault(r => r.Type == RewardType.Essence)?.Amount ?? 0;
        _exp = _idleRewards.Rewards.SingleOrDefault(r => r.Type == RewardType.Exp)?.Amount ?? 0;
        _items = _idleRewards.Rewards
            .Where(r => !new RewardType[] { RewardType.Gold, RewardType.Essence, RewardType.Exp }.Contains(r.Type))
            .ToDictionary(kvp => kvp.Id, kvp => kvp.Amount);

        IdleBattleManager.Instance.UpdateLastRewardTimestam(DateTime.Now);
        PopulateItems();
        UpdateText();
    }
    
    private void PopulateItems()
    {
        Dictionary<RewardId, int> d = new();

        if (!_items.Any())
        {
            itemsSection.SetActive(false);
            return;
        }

        itemsContainer.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (KeyValuePair<RewardId, int> reward in _items)
        {
            GameObject tokens = Instantiate(itemPrefab, itemsContainer);
            if (tokens.TryGetComponent(out ItemRewardPrefab item))
            {
                item.SetItemPrefab(reward.Key, reward.Value);
            }
        }
    }

    private void UpdateText()
    {
        idleTimeText.text = $"You idle rewards for {_idleRewards.IdleTime}";
        goldText.text = TextUtil.NumberFormatter(_gold);
        essenceText.text = TextUtil.NumberFormatter(_essence);
        expText.text = TextUtil.NumberFormatter(_exp);
    }

    public void AcceptRewards()
    {
        RewardService.ClaimRewards(_idleRewards.Rewards, IdleBattleManager.Instance.CurrentStage);
        gameObject.SetActive(false);
        PopupManager.Instance.ClosePopup(PopupId.AfkGains);
    }
}