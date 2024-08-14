using System;
using System.Collections.Generic;
using System.Linq;

public static class AfkGains
{
    public static int GetTimesCleared()
    {
        int secondsElapsed = Math.Min(GetAFKTimeInSeconds(), Consts.MaxIdleSecond);
        float partyDPS = Player.Instance.Party.PartyPower();
        float stageEHP = IdleBattleManager.Instance.GetStageEHP();
        float secondsToClear = stageEHP / partyDPS;
        int timesCleared = (int)(secondsElapsed / secondsToClear);
        return timesCleared;
    }

    public static IdleRewards CalculateRewards(int timesCleared)
    {
        IdleRewards idleRewards = new IdleRewards();
        List<Reward> rewards = new List<Reward>();
        
        Dictionary<MinimentalId, int> minimentals = IdleBattleManager.Instance.GetStage(IdleBattleManager.Instance.CurrentStage)
                    .GroupBy(id => id)
                    .ToDictionary(group => group.Key, group => group.Count());

        foreach (KeyValuePair<MinimentalId, int> kvp in minimentals)
        {
            Random random = new Random();
            MinimentalId id = kvp.Key;
            int countDefeated = kvp.Value * timesCleared;
            Minimental minimental = ElementalCatalog.Instance.GetElemental(id);

            foreach (Reward reward in minimental.Rewards)
            {
                for (int i = 0; i < countDefeated; i++)
                {
                    if (reward.Chance >= random.NextDouble())
                    {
                        rewards.Add(reward);
                    }
                }
            }
            
            idleRewards.IdleTime = TextUtil.FormatSecondsToTimeString(GetAFKTimeInSeconds());
        }

        idleRewards.Rewards = CompressRewards(rewards);
        return idleRewards;
    }
    
    private static int GetAFKTimeInSeconds()
    {
        return (int)(DateTime.Now - IdleBattleManager.Instance.LastRewardTimestamp).TotalSeconds;
    }
    
    private static List<Reward> CompressRewards(List<Reward> rewards)
    {
        return rewards
            .GroupBy(r => r.Id)
            .Select(g => new Reward
            {
                Id = g.Key,
                Type = g.First().Type,
                Amount = g.Sum(r => r.Amount),
                Chance = g.First().Chance
            })
            .ToList();
    }
}
