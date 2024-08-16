using System;
using System.Collections.Generic;

public static class RewardService
{
    private static readonly Dictionary<RewardId, ItemType> _itemType = new()
    {
        { RewardId.NormalBall, ItemType.Ball },
        { RewardId.GreatBall, ItemType.Ball },
        { RewardId.MasterBall, ItemType.Ball },
        { RewardId.FireToken, ItemType.Token },
        { RewardId.WaterToken, ItemType.Token },
        { RewardId.AirToken, ItemType.Token },
        { RewardId.EarthToken, ItemType.Token },
        { RewardId.LightningToken, ItemType.Token },
        { RewardId.IceToken, ItemType.Token },
        { RewardId.ChaosToken, ItemType.Token },
    };

    private static readonly Dictionary<RewardId, BallId> _ballMap = new()
    {
        { RewardId.NormalBall, BallId.NormalBall },
        { RewardId.GreatBall, BallId.GreatBall },
        { RewardId.MasterBall, BallId.MasterBall },
    };

    private static readonly Dictionary<RewardId, Elementoken> _tokenMap = new()
    {
        { RewardId.FireToken, Elementoken.FireToken },
        { RewardId.WaterToken, Elementoken.WaterToken },
        { RewardId.AirToken, Elementoken.AirToken },
        { RewardId.EarthToken, Elementoken.EarthToken },
        { RewardId.LightningToken, Elementoken.LightningToken },
        { RewardId.IceToken, Elementoken.IceToken },
        { RewardId.ChaosToken, Elementoken.ChaosToken },
    };

    public static void ClaimRewards(List<Reward> rewards, int level = 1)
    {
        rewards.ForEach(r => ClaimReward(r, level));
    }

    public static ItemType GetItemType(RewardId id)
    {
        return TranslateRewardId(_itemType, id);
    }

    public static void ClaimReward(Reward reward, int level = 1)
    {
        switch (reward.Type)
        {
            case RewardType.Gold:
                Player.Instance.Resources.UpdateGold(reward.Amount * level);
                break;
            case RewardType.Essence:
                Player.Instance.Resources.UpdateEssence(reward.Amount * level);
                break;
            case RewardType.Exp:
                Player.Instance.GainExperience(reward.Amount * level);
                break;
            case RewardType.Orbs:
                Player.Instance.Resources.UpdateOrbs(reward.Amount);
                break;
            case RewardType.Ball:
                Player.Instance.Inventory.UpdateBalls(TranslateRewardId(_ballMap, reward.Id), reward.Amount);
                break;
            case RewardType.Token:
                Player.Instance.Inventory.UpdateTokens(TranslateRewardId(_tokenMap, reward.Id), reward.Amount);
                break;
            default:
                throw new Exception($"RewardType {reward.Type} is not handled");
        }
    }

    private static T TranslateRewardId<T>(Dictionary<RewardId, T> map, RewardId id)
    {
        if (map.TryGetValue(id, out T value))
        {
            return value;
        }

        throw new Exception($"No {typeof(T).Name} mapped to {id}");
    }
}