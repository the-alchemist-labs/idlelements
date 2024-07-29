using System;

public class BattleEnemyPrefab : BaseBattlePrefab
{
    public void InitializeEnemy(MinimentalId minimentalId, int level)
    {
        Initialize(minimentalId, level);
        StartCoroutine(AttackRoutine());
    }

    protected override void HandlePostDefeat()
    {
        // rewards
    }
}
