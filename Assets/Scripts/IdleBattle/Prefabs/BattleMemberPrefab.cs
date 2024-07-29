using System;

public class BattleMemberPrefab : BaseBattlePrefab
{
    public void InitializeMember(ElementalId elementalId, int level)
    {
        Initialize(elementalId, level);

        if (AttackCoroutine != null)
        {
            StopCoroutine(AttackCoroutine);
        }
        AttackCoroutine = StartCoroutine(AttackRoutine());
    }

    protected override void HandlePostDefeat()
    {
        // add to battle stats?
    }
}
