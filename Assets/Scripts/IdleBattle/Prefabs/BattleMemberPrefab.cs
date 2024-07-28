using System;

public class BattleMemberPrefab : BaseBattlePrefab
{
    public void Initialize(ElementalId elementalId, int level)
    {
        BaseInitialize(elementalId, level, false);

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    protected override void HandlePostDefeat()
    {
        // add to battle stats?
    }
}
