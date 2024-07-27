using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBattlePrefab : MonoBehaviour
{
    const int HEALTH_PER_HP_MODIFIER = 3;

    public Elemental elemental { get; private set; }
    private List<SkillId> skills;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] protected Slider healthBar;

    protected Coroutine attackCoroutine;
    private bool isEnemyPrefab;

    public void Initialize(ElementalId elementalId, bool isEnemyPrefab)
    {
        this.isEnemyPrefab = isEnemyPrefab;
        elemental = ElementalCatalog.Instance.GetElemental(elementalId);
        skills = ElementalManager.Instance.GetSkills(elementalId);

        // summon animation
        sprite.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elementalId}");
        healthBar.maxValue = GetMaxHealth();
        healthBar.value = GetMaxHealth();
    }

    protected IEnumerator AttackRoutine()
    {
        while (true)
        {
            SkillId skillId = SelectNextSkill();
            ElementalSkill skill = SkillCatalog.Instance.GetSkill(skillId);
            Vector2? target = GetTargetLocation(skill.AttackTarget);

            if (target.HasValue)
            {
                UseSkill(skill, target.Value);
                yield return new WaitForSeconds(elemental.Stats.AttackSpeed);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private SkillId SelectNextSkill()
    {
        int random = Random.Range(0, skills.Count);
        return skills[random];
    }

    protected Vector2? GetTargetLocation(AttackTarget target)
    {
        Vector2 ownPosition = transform.position;

        if (target == AttackTarget.Self)
        {
            return ownPosition;
        }

        GameObject[] targets = GameObject.FindGameObjectsWithTag(GetTargetTag());
        if (targets.Length == 0)
            return null;

        GameObject closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject t in targets)
        {
            Vector2 targetPosition = t.transform.position;
            float distance = Vector2.Distance(ownPosition, targetPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = t;
            }
        }

        return closestTarget.transform.position;
    }

    private string GetTargetTag()
    {
        return isEnemyPrefab ? Tags.PartyMember : Tags.Enemy;
    }

    private void UseSkill(ElementalSkill skill, Vector2 target)
    {
        GameObject projectile = Instantiate(IdleBattleManager.Instance.prefab, transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectilePrefab>().Initialize(target, skill, elemental.Stats.Attack, GetTargetTag());
    }

    public void TakeDamage(int damageAmount)
    {
        healthBar.value -= damageAmount - elemental.Stats.Defense;

        if (healthBar.value <= 0) HandleDeath();
    }

    protected virtual void HandleDeath() { }

    protected int GetMaxHealth()
    {
        return elemental.Stats.Hp * HEALTH_PER_HP_MODIFIER;
    }
}
