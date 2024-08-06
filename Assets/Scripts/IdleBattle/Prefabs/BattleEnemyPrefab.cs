using System;
using System.Collections.Generic;
using System.Linq;

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
        RewardBalls(Elemental);
        RewardTokens(Elemental);
        
        IdleBattleManager.Instance.UpdateLastRewardTimestam(DateTime.Now);
    }

    private void RewardBalls(IElemental elemental)
    {
        Dictionary<BallId, int> balls = AfkGains.CalculateItems<BallId>(elemental.Rewards.Balls, 1);
        balls.ToList().ForEach(b => Player.Instance.Inventory.UpdateBalls(b.Key, b.Value));
    }

    private void RewardTokens(IElemental elemental)
    {
        Dictionary<ElementType, int> tokens = AfkGains.CalculateItems<ElementType>(elemental.Rewards.Elementokens, 1);
        tokens.ToList().ForEach(b => Player.Instance.Inventory.UpdateTokens(b.Key, b.Value));
    }
}
