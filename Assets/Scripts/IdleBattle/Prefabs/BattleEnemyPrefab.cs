using System;

public class BattleEnemyPrefab : BaseBattlePrefab
{
    public void Initialize(ElementalId elementalId, int level)
    {
        BaseInitialize(elementalId, level, true);
        StartCoroutine(AttackRoutine());
    }

    protected override void HandlePostDefeat()
    {
        // rewards
    }
}
