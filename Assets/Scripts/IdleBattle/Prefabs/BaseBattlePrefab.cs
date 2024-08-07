using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class BaseBattlePrefab : MonoBehaviour
{
    const int ATTACK_SPEED_INTERVAL_SECONDS = 1;

    public IElemental Elemental;
    public event Action<GameObject> OnDefeat;

    protected List<SkillId> Skills;
    protected Coroutine AttackCoroutine;
    protected int Level;

    [SerializeField] protected Slider HealthBar;
    [SerializeField] private SpriteRenderer _sprite;

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

    protected IEnumerator AttackRoutine()
    {
        while (true)
        {
            SkillId skillId = SelectNextSkill();
            Skill skill = ElementalCatalog.Instance.GetSkill(skillId);
            Vector2? target = GetTargetLocation(skill.AttackTarget);

            if (target.HasValue)
            {

                Vector2 resolvedTarget = target.Value;
                IdleBattleManager.Instance.ActivateSkill(
                    transform.position,
                    resolvedTarget, skill,
                    Elemental.Stats.Attack + Level,
                    GetTargetTag(skill.AttackTarget == AttackTarget.Self)
                );
                yield return new WaitForSeconds(ATTACK_SPEED_INTERVAL_SECONDS);
            }

            yield return new WaitForSeconds(ATTACK_SPEED_INTERVAL_SECONDS / 2);
        }
    }

    protected int GetMaxHealth()
    {
        return Elemental.Stats.Hp + Level;
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

    public void TakeDamage(int damageAmount)
    {
        HealthBar.value -= damageAmount - (Elemental.Stats.Defense + Level);

        if (HealthBar.value <= 0 && !_isDefeated)
        {
            HandleDefeat();
        }
    }

    private void BaseInitialize(int level)
    {
        Level = level;
        _isDefeated = false;

        HealthBar.maxValue = GetMaxHealth();
        HealthBar.value = GetMaxHealth();
    }

    private SkillId SelectNextSkill()
    {
        List<SkillId> skills = Skills.Where(s => s != SkillId.None).ToList();
        if (skills.Count == 0)
        {
            return SkillId.Default;
        }
        int random = UnityEngine.Random.Range(0, skills.Count);
        return skills[random];
    }

    private string GetTargetTag(bool isTargetingSelf)
    {
        if (isTargetingSelf)
        {
            return _isEnemyPrefab ? Tags.Enemy : Tags.PartyMember;
        }

        return _isEnemyPrefab ? Tags.PartyMember : Tags.Enemy;
    }

    private void HandleDefeat()
    {
        _isDefeated = true;
        HandlePostDefeat();
        OnDefeat?.Invoke(gameObject);
    }

    protected virtual void HandlePostDefeat() { }
}
