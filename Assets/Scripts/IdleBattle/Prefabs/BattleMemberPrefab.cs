using System;

public class BattleMemberPrefab : BaseBattlePrefab
{
    private Action<UnityEngine.GameObject> handleFaintedMember;

    public void Initialize(ElementalId elementalId, Action<UnityEngine.GameObject> handleFaintedMember)
    {
        this.handleFaintedMember = handleFaintedMember;
        BaseInitialize(elementalId, Player.Instance.Level, false);

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    protected override void HandleDeath()
    {
        handleFaintedMember(gameObject);
    }
}
