using System;

public class BattleEnemyPrefab : BaseBattlePrefab
{
    private Action<UnityEngine.GameObject> onEnemeyDeath;
    public void Initialize(ElementalId elementalId, int level, Action<UnityEngine.GameObject> onEnemeyDeath)
    {
        this.onEnemeyDeath = onEnemeyDeath;
        BaseInitialize(elementalId, level, true);
        StartCoroutine(AttackRoutine());
    }

    protected override void HandleDeath()
    {
        // disable (by manager)
        // rewards
        onEnemeyDeath(gameObject);
    }
}
