using System.Collections;
using UnityEngine;

public class BattleMemberPrefab : BaseBattlePrefab
{
    private const int DEATH_PENALTY_SECONDS = 5;
    private const int MOVE_X_DISTANCE = 100;

    public void SetElemental(ElementalId elementalId)
    {
        Initialize(elementalId, false);

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    protected override void HandleDeath()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            StartCoroutine(Respawn());
        }

        gameObject.tag = Tags.Untagged;
        transform.position = new Vector2(transform.position.x - MOVE_X_DISTANCE, transform.position.y);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(DEATH_PENALTY_SECONDS);
        gameObject.tag = Tags.PartyMember;
        transform.position = new Vector2(transform.position.x + MOVE_X_DISTANCE, transform.position.y);

        attackCoroutine = StartCoroutine(AttackRoutine());
        healthBar.value = GetMaxHealth();
    }
}
