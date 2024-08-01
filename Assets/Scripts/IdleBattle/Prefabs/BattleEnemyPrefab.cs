using System;
using System.Collections.Generic;

public class BattleEnemyPrefab : BaseBattlePrefab
{
    private int _level;

    public void InitializeEnemy(MinimentalId minimentalId, int level)
    {
        _level = level;
        Initialize(minimentalId, level);
        StartCoroutine(AttackRoutine());
    }

    protected override void HandlePostDefeat()
    {
        Player.Instance.GainExperience(Elemental.Rewards.Exp * _level);
        Player.Instance.Resources.UpdateEssence(Elemental.Rewards.Essence);
        Player.Instance.Resources.UpdateGold(Elemental.Rewards.Gold);
        RewardBalls(Elemental.Rewards.Balls);
    }

    private void RewardBalls(Dictionary<BallId, RewardsItem> balls)
    {
        Random random = new Random();

        foreach (KeyValuePair<BallId, RewardsItem> kvp in balls)
        {
            BallId ballId = kvp.Key;
            RewardsItem rewardsItem = kvp.Value;

            float randomNumber = (float)random.NextDouble();
            if (rewardsItem.Chance >= randomNumber)
            {
                Player.Instance.Inventory.UpdateBalls(ballId, rewardsItem.Amount);
            }
            print($"{ballId}: {rewardsItem.Amount}");

        }
    }
}
