using System;
using System.Collections.Generic;
using System.Linq;

public static class AfkGains
{
    static public int GetTimesCleared()
    {
        int secondsElapsed = Math.Min(GetAFKTimeInSeconds(), Consts.MaxIdleSecond);
        float partyDPS = Player.Instance.Party.PartyPower();
        float stageEHP = IdleBattleManager.Instance.GetStageEHP();
        float secondsToClear = stageEHP / partyDPS;
        int timesCleared = (int)(secondsElapsed / secondsToClear);
        return timesCleared;
    }

    static public IdleRewards CalculateRewards(int timesCleared)
    {
        IdleRewards rewards = new IdleRewards();
        Dictionary<MinimentalId, int> minimentals = IdleBattleManager.Instance.GetStage(IdleBattleManager.Instance.CurrentStage)
                    .GroupBy(id => id)
                    .ToDictionary(group => group.Key, group => group.Count());

        foreach (KeyValuePair<MinimentalId, int> kvp in minimentals)
        {
            MinimentalId id = kvp.Key;
            int countDefeated = kvp.Value * timesCleared;
            int level = IdleBattleManager.Instance.CurrentStage;

            Minimental minimental = ElementalCatalog.Instance.GetElemental(id);

            rewards.Experience += minimental.Rewards.Exp * level * countDefeated;
            rewards.Essence += minimental.Rewards.Essence * level * countDefeated;
            rewards.Gold += minimental.Rewards.Gold * level * countDefeated;

            rewards.Balls = CalculateBalls(minimental, countDefeated);
            rewards.IdleTime = TextUtil.FormatSecondsToTimeString(GetAFKTimeInSeconds());
        }

        return rewards;
    }

    static public void AcceptRewards(IdleRewards rewards)
    {
        Player.Instance.GainExperience(rewards.Experience);
        Player.Instance.Resources.UpdateEssence(rewards.Essence);
        Player.Instance.Resources.UpdateGold(rewards.Gold);

        foreach (KeyValuePair<BallId, int> kvp in rewards.Balls)
        {
            Player.Instance.Inventory.UpdateBalls(kvp.Key, kvp.Value);
        }
    }

    static private Dictionary<BallId, int> CalculateBalls(Minimental minimental, int countDefeated)
    {
        Dictionary<BallId, int> ballsRewards = new Dictionary<BallId, int>();
        Random random = new Random();

        foreach (BallReward ballReward in minimental.Rewards.Balls)
        {
            int ballsGained = 0;
            
            for (int i = 0; i < countDefeated; i++)
            {
                float randomNumber = (float)random.NextDouble();
                if (ballReward.Chance >= randomNumber)
                {
                    ballsGained += ballReward.Amount;
                }
            }

            if (ballsRewards.ContainsKey(ballReward.BallId))
            {
                ballsRewards[ballReward.BallId] += ballsGained;
            }
            else
            {
                ballsRewards.Add(ballReward.BallId, ballsGained);
            }
        }

        return ballsRewards;
    }

    static private int GetAFKTimeInSeconds()
    {
        return (int)(DateTime.Now - IdleBattleManager.Instance.LastRewardTimestamp).TotalSeconds;
    }
}
