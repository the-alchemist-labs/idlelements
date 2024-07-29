using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBattlePrefab : MonoBehaviour
{
    const int HEALTH_PER_HP_MODIFIER = 3;

    public IElemental Elemental;
    public event Action<GameObject> OnDefeat;

    protected List<SkillId> Skills;
    protected Coroutine AttackCoroutine;

    [SerializeField] protected Slider HealthBar;
    [SerializeField] private SpriteRenderer _sprite;

    private int _level;
    private bool _isEnemyPrefab;
    private bool _isDefeated;

    protected void Initialize(ElementalId id, int level)
    {
        _isEnemyPrefab = false;
        _sprite.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{id}");
        Elemental = ElementalCatalog.Instance.GetElemental(id);
        Skills = ElementalManager.Instance.GetSkills(id);
        BaseInitialize(level);
    }

    protected void Initialize(MinimentalId id, int level)
    {
        _isEnemyPrefab = true;
        Elemental = ElementalCatalog.Instance.GetElemental(id);
        Skills = ElementalManager.Instance.GetSkills(id);
        _sprite.sprite = Resources.Load<Sprite>($"Sprites/Minimentals/{id}");
        BaseInitialize(level);
    }

    private void BaseInitialize(int level)
    {
        // summon animation
        _level = level;
        _isDefeated = false;

        HealthBar.maxValue = GetMaxHealth();
        HealthBar.value = GetMaxHealth();
    }

    protected IEnumerator AttackRoutine()
    {
        while (true)
        {
            SkillId skillId = SelectNextSkill();
            ElementalSkill skill = ElementalCatalog.Instance.GetSkill(skillId);
            Vector2? target = GetTargetLocation(skill.AttackTarget);

            if (target.HasValue)
            {

                Vector2 resolvedTarget = target.Value;
                IdleBattleManager.Instance.ActivateSkill(
                    transform.position,
                    resolvedTarget, skill,
                    Elemental.Stats.Attack * _level,
                    GetTargetTag(skill.AttackTarget == AttackTarget.Self)
                );
                yield return new WaitForSeconds(Elemental.Stats.AttackSpeed);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private SkillId SelectNextSkill()
    {
        int random = UnityEngine.Random.Range(0, Skills.Count);
        return Skills[random];
    }

    protected Vector2? GetTargetLocation(AttackTarget target)
    {
        bool isTargetingSelf = target == AttackTarget.Self;
        Vector2 ownPosition = transform.position;

        if (isTargetingSelf)
        {
            return ownPosition;
        }

        GameObject[] targets = GameObject.FindGameObjectsWithTag(GetTargetTag(isTargetingSelf));
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

    private string GetTargetTag(bool isTargetingSelf)
    {
        if (isTargetingSelf)
        {
            return _isEnemyPrefab ? Tags.Enemy : Tags.PartyMember;
        }

        return _isEnemyPrefab ? Tags.PartyMember : Tags.Enemy;
    }

    public void TakeDamage(int damageAmount)
    {
        HealthBar.value -= damageAmount - Elemental.Stats.Defense * _level;

        if (HealthBar.value <= 0 && !_isDefeated)
        {
            HandleDefeat();
        }
    }

    private void HandleDefeat()
    {
        _isDefeated = true;
        OnDefeat?.Invoke(gameObject);
        HandlePostDefeat();
    }

    protected int GetMaxHealth()
    {
        return Elemental.Stats.Hp * HEALTH_PER_HP_MODIFIER;
    }

    protected virtual void HandlePostDefeat() { }
}
