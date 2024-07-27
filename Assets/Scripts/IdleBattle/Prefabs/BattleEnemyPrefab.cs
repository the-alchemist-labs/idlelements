public class BattleEnemyPrefab : BaseBattlePrefab
{
    public void SetEnemy(ElementalId elementalId)
    {
        Initialize(elementalId, true);
        StartCoroutine(AttackRoutine());
    }

    protected override void HandleDeath()
    {
        // disable (by manager)
        // rewards
        Destroy(gameObject);
    }
}
