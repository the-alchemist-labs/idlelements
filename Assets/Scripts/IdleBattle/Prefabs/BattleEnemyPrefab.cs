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
        if (Elemental.Rewards.Ball != BallId.None)
        {
            Player.Instance.Inventory.UpdateBalls(Elemental.Rewards.Ball, 1);
        }
    }
}
