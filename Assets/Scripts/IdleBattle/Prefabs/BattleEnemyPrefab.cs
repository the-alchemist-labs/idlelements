using System;
using System.Collections.Generic;

public class BattleEnemyPrefab : BaseBattlePrefab
{
    public void InitializeEnemy(MinimentalId minimentalId, int level)
    {
        Level = level;
        Initialize(minimentalId, level);
        StartCoroutine(AttackRoutine());
    }

    protected override void HandlePostDefeat()
    {
        Player.Instance.GainExperience(Elemental.Rewards.Exp * Level);
        Player.Instance.Resources.UpdateEssence(Elemental.Rewards.Essence);
        Player.Instance.Resources.UpdateGold(Elemental.Rewards.Gold);
        RewardBalls(Elemental.Rewards.Balls);
        IdleBattleManager.Instance.UpdateLastRewardTimestam(DateTime.Now);
    }

    private void RewardBalls(List<BallReward> balls)
    {
        Random random = new Random();

        foreach (BallReward ballReward in balls)
        {
            float randomNumber = (float)random.NextDouble();
            if (ballReward.Chance >= randomNumber)
            {
                Player.Instance.Inventory.UpdateBalls(ballReward.BallId, ballReward.Amount);
            }
        }
    }
}
