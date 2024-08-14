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
        RewardService.ClaimRewards(Elemental.Rewards, Level);
        IdleBattleManager.Instance.UpdateLastRewardTimestam(DateTime.Now);
    }
}
